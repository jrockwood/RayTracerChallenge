import { Light } from './Lights';
import { IntersectionList, Ray } from './Ray';
import { Shape } from './Shapes';

export class World {
  public readonly light: Light;
  public readonly shapes: ReadonlyArray<Shape>;

  public constructor(light: Light, shapes: ReadonlyArray<Shape>) {
    this.light = light;
    this.shapes = shapes;
  }

  public intersect(ray: Ray): IntersectionList {
    let hits = new IntersectionList();

    this.shapes.forEach((shape) => {
      const shapeHits = shape.intersect(ray);
      hits = hits.add(...shapeHits.values);
    });

    return hits;
  }
}

export class WorldBuilder {
  public light?: Light;
  public shapes: Shape[] = [];

  public build(): World {
    if (!this.light) {
      throw new Error('No light has been added to the world');
    }

    return new World(this.light, this.shapes);
  }
}
