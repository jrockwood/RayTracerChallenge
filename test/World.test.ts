import { Color } from '../src/Color';
import { PointLight } from '../src/Lights';
import { Material } from '../src/Materials';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';
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
});

function createDefaultWorld(): World {
  const light = new PointLight(new Point(-10, 10, -10), Color.White);
  const sphere1 = new Sphere(undefined, new Material(new Color(0.8, 1.0, 0.6), undefined, 0.7, 0.2));
  const sphere2 = new Sphere(Matrix4x4.scaling(0.5, 0.5, 0.5));
  return new World(light, [sphere1, sphere2]);
}
