import { IntersectionList, Intersection } from './Intersections';
import { Material } from './Materials';
import { EPSILON } from './Math';
import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';
import { Ray } from './Ray';

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Shape

export abstract class Shape {
  public readonly transform: Matrix4x4;
  public readonly material: Material;
  public readonly ignoreShadow: boolean;

  protected constructor(
    transform: Matrix4x4 = Matrix4x4.identity,
    material: Material = new Material(),
    ignoreShadow = false,
  ) {
    this.transform = transform;
    this.material = material;
    this.ignoreShadow = ignoreShadow;
  }

  public intersect(ray: Ray): IntersectionList {
    const localRay = ray.transform(this.transform.inverse());
    return this.localIntersect(localRay);
  }
  protected abstract localIntersect(localRay: Ray): IntersectionList;

  public normalAt(worldPoint: Point): Vector {
    const localPoint = this.transform.inverse().multiplyByPoint(worldPoint);
    const localNormal = this.localNormalAt(localPoint);
    const worldNormal = this.transform.inverse().transpose().multiplyByVector(localNormal);
    return worldNormal.normalize();
  }

  protected abstract localNormalAt(localPoint: Point): Vector;

  public abstract withTransform(value: Matrix4x4): Shape;
  public abstract withMaterial(value: Material): Shape;
  public abstract withIgnoreShadow(value: boolean): Shape;

  public addToMaterial(addFunc: (currentMaterial: Material) => Material): Shape {
    return this.withMaterial(addFunc(this.material));
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sphere

export class Sphere extends Shape {
  public constructor(transform?: Matrix4x4, material?: Material, ignoreShadow?: boolean) {
    super(transform, material, ignoreShadow);
  }

  public withTransform(value: Matrix4x4): Shape {
    return new Sphere(value, this.material);
  }

  public withMaterial(value: Material): Shape {
    return new Sphere(this.transform, value);
  }

  public withIgnoreShadow(value: boolean): Shape {
    return new Sphere(this.transform, this.material, value);
  }

  protected localIntersect(localRay: Ray): IntersectionList {
    const sphereToRay: Vector = localRay.origin.subtract(new Point(0, 0, 0));

    // Calculate the discriminant using this formula: b^2 - 4ac
    const a = localRay.direction.dot(localRay.direction);
    const b = 2 * localRay.direction.dot(sphereToRay);
    const c = sphereToRay.dot(sphereToRay) - 1;
    const discriminant = b * b - 4 * a * c;

    if (discriminant < 0) {
      return new IntersectionList();
    }

    const sqrtOfDiscriminant = Math.sqrt(discriminant);
    const t1 = (-b - sqrtOfDiscriminant) / (2 * a);
    const t2 = (-b + sqrtOfDiscriminant) / (2 * a);

    return new IntersectionList(new Intersection(t1, this), new Intersection(t2, this));
  }

  protected localNormalAt(localPoint: Point): Vector {
    const localNormal = localPoint.subtract(Point.zero);
    return localNormal;
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Plane

export class Plane extends Shape {
  public constructor(transform?: Matrix4x4, material?: Material, ignoreShadow?: boolean) {
    super(transform, material, ignoreShadow);
  }

  protected localIntersect(localRay: Ray): IntersectionList {
    // If the ray is parallel to the plane, there are no intersections.
    if (Math.abs(localRay.direction.y) < EPSILON) {
      return new IntersectionList();
    }

    const t = -localRay.origin.y / localRay.direction.y;
    return new IntersectionList(new Intersection(t, this));
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  protected localNormalAt(localPoint: Point): Vector {
    return new Vector(0, 1, 0);
  }

  public withTransform(value: Matrix4x4): Shape {
    return new Plane(value, this.material);
  }

  public withMaterial(value: Material): Shape {
    return new Plane(this.transform, value);
  }

  public withIgnoreShadow(value: boolean): Shape {
    return new Plane(this.transform, this.material, value);
  }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Cube

export class Cube extends Shape {
  public constructor(transform?: Matrix4x4, material?: Material, ignoreShadow?: boolean) {
    super(transform, material, ignoreShadow);
  }

  protected localIntersect(localRay: Ray): IntersectionList {
    function checkAxis(origin: number, direction: number): { tmin: number; tmax: number } {
      const tminNumerator = -1 - origin;
      const tmaxNumerator = 1 - origin;
      let tmin: number;
      let tmax: number;

      if (Math.abs(direction) >= EPSILON) {
        tmin = tminNumerator / direction;
        tmax = tmaxNumerator / direction;
      } else {
        tmin = tminNumerator * Number.POSITIVE_INFINITY;
        tmax = tmaxNumerator * Number.POSITIVE_INFINITY;
      }

      if (tmin > tmax) {
        const temp = tmin;
        tmin = tmax;
        tmax = temp;
      }

      return { tmin, tmax };
    }

    const { tmin: xtMin, tmax: xtMax } = checkAxis(localRay.origin.x, localRay.direction.x);
    const { tmin: ytMin, tmax: ytMax } = checkAxis(localRay.origin.y, localRay.direction.y);
    const { tmin: ztMin, tmax: ztMax } = checkAxis(localRay.origin.z, localRay.direction.z);

    const tmin = Math.max(xtMin, ytMin, ztMin);
    const tmax = Math.min(xtMax, ytMax, ztMax);

    if (tmin > tmax) {
      return new IntersectionList();
    }

    return new IntersectionList(new Intersection(tmin, this), new Intersection(tmax, this));
  }

  protected localNormalAt(localPoint: Point): Vector {
    const maxc = Math.max(Math.abs(localPoint.x), Math.abs(localPoint.y), Math.abs(localPoint.z));

    if (maxc === Math.abs(localPoint.x)) {
      return new Vector(localPoint.x, 0, 0);
    } else if (maxc === Math.abs(localPoint.y)) {
      return new Vector(0, localPoint.y, 0);
    }

    return new Vector(0, 0, localPoint.z);
  }

  public withTransform(value: Matrix4x4): Shape {
    return new Cube(value, this.material, this.ignoreShadow);
  }

  public withMaterial(value: Material): Shape {
    return new Cube(this.transform, value, this.ignoreShadow);
  }

  public withIgnoreShadow(value: boolean): Shape {
    return new Cube(this.transform, this.material, value);
  }
}
