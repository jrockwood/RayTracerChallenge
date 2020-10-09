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

    return surfaceColor.add(reflectedColor);
  }

  public colorAt(ray: Ray, maxRecursion = World.maxRecursion): Color {
    const intersections = this.intersect(ray);
    const hit = intersections.hit();

    if (!hit) {
      return Color.Black;
    }

    const comps = hit.prepareComputations(ray);
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
}
