import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';
import { Shape } from './Shapes';

export class Ray {
  public readonly origin: Point;
  public readonly direction: Vector;

  public constructor(origin: Point, direction: Vector) {
    this.origin = origin;
    this.direction = direction;
  }

  public position(t: number): Point {
    return this.origin.add(this.direction.multiply(t));
  }

  public transform(matrix: Matrix4x4): Ray {
    const newOrigin = matrix.multiplyByPoint(this.origin);
    const newDirection = matrix.multiplyByVector(this.direction);
    return new Ray(newOrigin, newDirection);
  }
}

export interface PrecomputedIntersectionState {
  t: number;
  shape: Shape;
  point: Point;
  eye: Vector;
  normal: Vector;
  isInside: boolean;
}

export class Intersection {
  public readonly t: number;
  public readonly shape: Shape;

  public constructor(t: number, shape: Shape) {
    this.t = t;
    this.shape = shape;
  }

  public prepareComputations(ray: Ray): PrecomputedIntersectionState {
    const point = ray.position(this.t);
    const eye = ray.direction.negate();
    let normal = this.shape.normalAt(point);
    let isInside = false;

    if (normal.dot(eye) < 0) {
      isInside = true;
      normal = normal.negate();
    }

    return {
      t: this.t,
      shape: this.shape,
      point,
      eye,
      normal,
      isInside,
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