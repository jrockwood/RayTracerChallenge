import { Material } from './Materials';
import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';
import { Intersection, IntersectionList, Ray } from './Ray';

export abstract class Shape {
  public readonly transform: Matrix4x4;
  public readonly material: Material;

  protected constructor(transform: Matrix4x4, material: Material) {
    this.transform = transform;
    this.material = material;
  }
}

export class Sphere extends Shape {
  public constructor(transform: Matrix4x4 = Matrix4x4.identity, material: Material = new Material()) {
    super(transform, material);
  }

  public intersect(ray: Ray): IntersectionList {
    const transformedRay = ray.transform(this.transform.inverse());

    const sphereToRay: Vector = transformedRay.origin.subtract(new Point(0, 0, 0));

    // Calculate the discriminant using this formula: b^2 - 4ac
    const a = transformedRay.direction.dot(transformedRay.direction);
    const b = 2 * transformedRay.direction.dot(sphereToRay);
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

  public normalAt(worldPoint: Point): Vector {
    const objectPoint = this.transform.inverse().multiplyByPoint(worldPoint);
    const objectNormal = objectPoint.subtract(Point.zero);
    const worldNormal = this.transform.inverse().transpose().multiplyByVector(objectNormal);
    return worldNormal.normalize();
  }
}
