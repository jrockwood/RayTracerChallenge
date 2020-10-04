import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';
import { Sphere } from '../src/Shapes';

describe('Sphere', () => {
  describe('Ray intersections', () => {
    it('a ray intersects a sphere at two points', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.map((x) => x.t)).toEqual([4.0, 6.0]);
    });

    it('sets the shape on the interection', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.map((x) => x.shape)).toEqual([sphere, sphere]);
    });

    it('a ray intersects a sphere at a tangent', () => {
      const ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.map((x) => x.t)).toEqual([5.0, 5.0]);
    });

    it('a ray misses a sphere', () => {
      const ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.length).toBe(0);
    });

    it('a ray originates inside a sphere', () => {
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.map((x) => x.t)).toEqual([-1.0, 1.0]);
    });

    it('a sphere is behind a ray', () => {
      const ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersects(ray);
      expect(points.map((x) => x.t)).toEqual([-6.0, -4.0]);
    });
  });
});
