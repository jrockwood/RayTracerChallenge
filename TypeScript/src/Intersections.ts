import { EPSILON } from './Math';
import { Point, Vector } from './PointVector';
import { Ray } from './Ray';
import { Shape } from './Shapes';

export interface PrecomputedIntersectionState {
  t: number;
  shape: Shape;
  point: Point;
  eye: Vector;
  normal: Vector;
  isInside: boolean;
  overPoint: Point;
  underPoint: Point;
  reflect: Vector;
  n1: number;
  n2: number;
}

export class Intersection {
  public readonly t: number;
  public readonly shape: Shape;

  public constructor(t: number, shape: Shape) {
    this.t = t;
    this.shape = shape;
  }

  public prepareComputations(ray: Ray, intersections: IntersectionList): PrecomputedIntersectionState {
    const point = ray.position(this.t);
    const eye = ray.direction.negate();
    let normal = this.shape.normalAt(point);
    let isInside = false;

    if (normal.dot(eye) < 0) {
      isInside = true;
      normal = normal.negate();
    }

    const overPoint = point.add(normal.multiply(EPSILON));
    const underPoint = point.subtractVector(normal.multiply(EPSILON));

    const reflect = ray.direction.reflect(normal);

    // Calculate the refraction values.
    const containers: Shape[] = [];
    let n1 = 1.0;
    let n2 = 1.0;

    intersections.values.forEach((intersection) => {
      if (intersection === this) {
        if (containers.length === 0) {
          n1 = 1.0;
        } else {
          n1 = containers[containers.length - 1].material.refractiveIndex;
        }
      }

      const index = containers.indexOf(intersection.shape);
      if (index >= 0) {
        containers.splice(index, 1);
      } else {
        containers.push(intersection.shape);
      }

      if (intersection === this) {
        if (containers.length === 0) {
          n2 = 1.0;
        } else {
          n2 = containers[containers.length - 1].material.refractiveIndex;
        }
      }
    });

    return {
      t: this.t,
      shape: this.shape,
      point,
      eye,
      normal,
      isInside,
      overPoint,
      underPoint,
      reflect,
      n1,
      n2,
    };
  }
}

export class IntersectionList {
  private _intersections: ReadonlyArray<Intersection>;

  public constructor(...intersections: Intersection[]) {
    this._intersections = intersections.sort((a, b) => a.t - b.t);
  }

  public get length(): number {
    return this._intersections.length;
  }

  public get values(): Intersection[] {
    return [...this._intersections];
  }

  public get ts(): number[] {
    return this._intersections.map((x) => x.t);
  }

  public get shapes(): Shape[] {
    return this._intersections.map((x) => x.shape);
  }

  public get(index: number): Intersection {
    if (index < 0 || index >= this._intersections.length) {
      throw new Error(`Index out of bounds: ${index}`);
    }

    return this._intersections[index];
  }

  public add(...intersctions: Intersection[]): IntersectionList {
    return new IntersectionList(...[...this._intersections, ...intersctions]);
  }

  public hit(): Intersection | null {
    const nonNegativeTs = this._intersections.filter((x) => x.t >= 0);
    return nonNegativeTs.length > 0 ? nonNegativeTs[0] : null;
  }
}

export function schlick(comps: PrecomputedIntersectionState): number {
  // Find the cosine of the angle between the eye and the normal vector.
  let cos = comps.eye.dot(comps.normal);

  // Total internal reflection can only occur if n1 > n2.
  if (comps.n1 > comps.n2) {
    const n = comps.n1 / comps.n2;
    const sin2_t = n * n * (1.0 - cos * cos);
    if (sin2_t > 1.0) {
      return 1.0;
    }

    // Compute the cosine of theta_t using trig identity.
    const cos_t = Math.sqrt(1.0 - sin2_t);

    // When n1 > n2, use cos(theta_t) instead.
    cos = cos_t;
  }

  const r0 = Math.pow((comps.n1 - comps.n2) / (comps.n1 + comps.n2), 2);
  return r0 + (1 - r0) * Math.pow(1 - cos, 5);
}
