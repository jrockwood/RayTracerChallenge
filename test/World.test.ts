import { Color } from '../src/Color';
import { Light, PointLight } from '../src/Lights';
import { Material } from '../src/Materials';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Intersection, Ray } from '../src/Ray';
import { Sphere } from '../src/Shapes';
import { World } from '../src/World';

describe('World', () => {
  describe('ctor()', () => {
    it('should store the light source and the shapes', () => {
      const light = new PointLight(new Point(-10, 10, -10), Color.White);
      const sphere1 = new Sphere(undefined, new Material(new Color(0.8, 1.0, 0.6), undefined, 0.7, 0.2));
      const sphere2 = new Sphere(Matrix4x4.scaling(0.5, 0.5, 0.5));
      const world = new World(light, [sphere1, sphere2]);
      expect(world.light).toBe(light);
      expect(world.shapes).toEqual([sphere1, sphere2]);
    });
  });

  describe('intersect()', () => {
    it('should intersect with both spheres when looking at the origin', () => {
      const world = createDefaultWorld();
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const intersections = world.intersect(ray);
      expect(intersections.ts).toEqual([4, 4.5, 5.5, 6]);
    });
  });

  describe('shadeHit()', () => {
    it('should shade an intersection', () => {
      const world = createDefaultWorld();
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const shape = world.shapes[0];
      const intersection = new Intersection(4, shape);
      const comps = intersection.prepareComputations(ray);
      const color = world.shadeHit(comps);
      expect(color.isEqualTo(new Color(0.38066, 0.47583, 0.2855))).toBeTrue();
    });

    it('should shade an intersection from the inside', () => {
      const world = createDefaultWorld().withLight(new PointLight(new Point(0, 0.25, 0), Color.White));
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const shape = world.shapes[1];
      const intersection = new Intersection(0.5, shape);
      const comps = intersection.prepareComputations(ray);
      const color = world.shadeHit(comps);
      expect(color.isEqualTo(new Color(0.90498, 0.90498, 0.90498))).toBeTrue();
    });
  });

  describe('colorAt()', () => {
    it('should return black when a ray misses', () => {
      const world = createDefaultWorld();
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
      const color = world.colorAt(ray);
      expect(color).toEqual(Color.Black);
    });

    it('should return the color when a ray hits', () => {
      const world = createDefaultWorld();
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const color = world.colorAt(ray);
      expect(color.isEqualTo(new Color(0.38066, 0.47583, 0.2855))).toBeTrue();
    });

    it('should return the color with an intersection behind the ray', () => {
      let world = createDefaultWorld();
      let outer = world.shapes[0];
      let inner = world.shapes[1];
      outer = outer.withMaterial(outer.material.withAmbient(1));
      inner = inner.withMaterial(inner.material.withAmbient(1));
      world = world.withShapes([outer, inner]);

      const ray = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));
      const color = world.colorAt(ray);
      expect(color).toEqual(inner.material.color);
    });
  });
});

export function createDefaultWorld(): World {
  const light = new PointLight(new Point(-10, 10, -10), Color.White);
  const sphere1 = new Sphere(undefined, new Material(new Color(0.8, 1.0, 0.6), undefined, 0.7, 0.2));
  const sphere2 = new Sphere(Matrix4x4.scaling(0.5, 0.5, 0.5));
  return new World(light, [sphere1, sphere2]);
}