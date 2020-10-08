import { Color } from './Color';
import { Light } from './Lights';
import { Point } from './PointVector';
import { IntersectionList, PrecomputedIntersectionState, Ray } from './Ray';
import { Shape } from './Shapes';

export class World {
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

  public intersect(ray: Ray): IntersectionList {
    let hits = new IntersectionList();

    this.shapes.forEach((shape) => {
      const shapeHits = shape.intersect(ray);
      hits = hits.add(...shapeHits.values);
    });

    return hits;
  }

  public shadeHit(comps: PrecomputedIntersectionState): Color {
    const isShadowed = this.isShadowed(comps.overPoint);
    const color = comps.shape.material.lighting(
      comps.shape,
      this.light,
      comps.point,
      comps.eye,
      comps.normal,
      isShadowed,
    );
    return color;
  }

  public colorAt(ray: Ray): Color {
    const intersections = this.intersect(ray);
    const hit = intersections.hit();

    if (!hit) {
      return Color.Black;
    }

    const comps = hit.prepareComputations(ray);
    const color = this.shadeHit(comps);
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
}
