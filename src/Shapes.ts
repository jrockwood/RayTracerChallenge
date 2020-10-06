import { Material } from './Materials';
import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';
import { Intersection, IntersectionList, Ray } from './Ray';

export abstract class Shape {
  public readonly transform: Matrix4x4;
  public readonly material: Material;

  protected constructor(transform: Matrix4x4 = Matrix4x4.identity, material: Material = new Material()) {
    this.transform = transform;
    this.material = material;
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
}

export class Sphere extends Shape {
  public constructor(transform?: Matrix4x4, material?: Material) {
    super(transform, material);
  }

  public withTransform(value: Matrix4x4): Shape {
    return new Sphere(value, this.material);
  }

  public withMaterial(value: Material): Shape {
    return new Sphere(this.transform, value);
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
