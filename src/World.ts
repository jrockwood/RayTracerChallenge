import { Color } from './Color';
import { Light } from './Lights';
import { Point } from './PointVector';
import { IntersectionList, PrecomputedIntersectionState, Ray } from './Ray';
import { Shape } from './Shapes';

export class World {
  public static readonly maxRecursion = 5;

  public readonly light: Light;
  public readonly shapes: ReadonlyArray<Shape>;

  public constructor(light: Light, shapes: ReadonlyArray<Shape>) {
    this.light = light;
    this.shapes = shapes;
  }

  public withLight(value: Light): World {
    return new World(value, this.shapes);
  }

  public withShapes(value: ReadonlyArray<Shape>): World {
    return new World(this.light, value);
  }

  public addShape(shape: Shape): World {
    return new World(this.light, [...this.shapes, shape]);
  }

  public intersect(ray: Ray): IntersectionList {
    let hits = new IntersectionList();

    this.shapes.forEach((shape) => {
      const shapeHits = shape.intersect(ray);
      hits = hits.add(...shapeHits.values);
    });

    return hits;
  }

  public shadeHit(comps: PrecomputedIntersectionState, maxRecursion = World.maxRecursion): Color {
    const isShadowed = this.isShadowed(comps.overPoint);
    const surfaceColor = comps.shape.material.lighting(
      comps.shape,
      this.light,
      comps.overPoint,
      comps.eye,
      comps.normal,
      isShadowed,
    );

    const reflectedColor = this.reflectedColor(comps, maxRecursion);
    const refractedColor = this.refractedColor(comps, maxRecursion);

    return surfaceColor.add(reflectedColor).add(refractedColor);
  }

  public colorAt(ray: Ray, maxRecursion = World.maxRecursion): Color {
    const intersections = this.intersect(ray);
    const hit = intersections.hit();

    if (!hit) {
      return Color.Black;
    }

    const comps = hit.prepareComputations(ray, intersections);
    const color = this.shadeHit(comps, maxRecursion);
    return color;
  }

  public isShadowed(point: Point): boolean {
    const pointToLightVector = this.light.position.subtract(point);
    const distance = pointToLightVector.magnitude();
    const direction = pointToLightVector.normalize();

    const ray = new Ray(point, direction);
    const intersections = this.intersect(ray);

    const hit = intersections.hit();
    const isShadowed = hit !== null && hit.t < distance;
    return isShadowed;
  }

  public reflectedColor(comps: PrecomputedIntersectionState, maxRecursion = World.maxRecursion): Color {
    if (maxRecursion <= 0 || comps.shape.material.reflective === 0) {
      return Color.Black;
    }

    const reflectRay = new Ray(comps.overPoint, comps.reflect);
    const color = this.colorAt(reflectRay, maxRecursion - 1);
    const reflectedColor = color.multiply(comps.shape.material.reflective);
    return reflectedColor;
  }

  public refractedColor(comps: PrecomputedIntersectionState, maxRecursion = World.maxRecursion): Color {
    if (maxRecursion <= 0 || comps.shape.material.transparency === 0) {
      return Color.Black;
    }

    // Fine the ratio of the first index of refraction to the second. This is inverted from the
    // definition of Snell's Law, which is sin(theta_i) / sin(theta_t) = n2 / n1.
    const nRatio = comps.n1 / comps.n2;

    // cos(theta_i) is the same as the dot product of the two vectors.
    const cos_i = comps.eye.dot(comps.normal);

    // Find sin(theta_t)^2 via trigonometric identity.
    const sin2_t = nRatio * nRatio * (1 - cos_i * cos_i);

    // Total internal reflection - there is no refraction.
    if (sin2_t > 1) {
      return Color.Black;
    }

    // Find cos(theta_t) via trigonometric identity.
    const cos_t = Math.sqrt(1.0 - sin2_t);

    // Compute the direction of the refracted ray.
    const direction = comps.normal.multiply(nRatio * cos_i - cos_t).subtract(comps.eye.multiply(nRatio));

    // Create the refracted ray.
    const refractRay = new Ray(comps.underPoint, direction);

    // Find the color of the refracted ray, making sure to multiply by the transparency value to
    // account for any opacity.
    const color = this.colorAt(refractRay, maxRecursion - 1).multiply(comps.shape.material.transparency);

    return color;
  }
}
