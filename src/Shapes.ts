import { Point, Vector } from './PointVector';
import { Intersection, Ray } from './Ray';

export abstract class Shape {}

export class Sphere extends Shape {
  public intersects(ray: Ray): Intersection[] {
    // Calculate the vector from the sphere's center to the ray origin. Remember: the sphere is
    // centered at the world origin.
    const sphereToRay: Vector = ray.origin.subtract(new Point(0, 0, 0));

    // Calculate the discriminant using this formula: b^2 - 4ac
    const a = ray.direction.dot(ray.direction);
    const b = 2 * ray.direction.dot(sphereToRay);
    const c = sphereToRay.dot(sphereToRay) - 1;
    const discriminant = b * b - 4 * a * c;

    if (discriminant < 0) {
      return [];
    }

    const sqrtOfDiscriminant = Math.sqrt(discriminant);
    const t1 = (-b - sqrtOfDiscriminant) / (2 * a);
    const t2 = (-b + sqrtOfDiscriminant) / (2 * a);

    return [new Intersection(Math.min(t1, t2), this), new Intersection(Math.max(t1, t2), this)];
  }
}
