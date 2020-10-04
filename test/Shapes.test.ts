import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';
import { Sphere } from '../src/Shapes';

describe('Sphere', () => {
  describe('ctor()', () => {
    it('should default to the identity matrix for the transform', () => {
      const sphere = new Sphere();
      expect(sphere.transform).toEqual(Matrix4x4.identity);
    });

    it('should store the transform', () => {
      const sphere = new Sphere(Matrix4x4.translation(2, 3, 4));
      expect(sphere.transform).toEqual(Matrix4x4.translation(2, 3, 4));
    });
  });

  describe('Ray intersections with a sphere', () => {
    it('should intersect at two points', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.ts).toEqual([4.0, 6.0]);
    });

    it('should set the shape on the interection', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.shapes).toEqual([sphere, sphere]);
    });

    it('should intersect at a tangent', () => {
      const ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.ts).toEqual([5.0, 5.0]);
    });

    it('should miss correctly', () => {
      const ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.length).toBe(0);
    });

    it('should hit twice when a ray originates inside a sphere', () => {
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.ts).toEqual([-1.0, 1.0]);
    });

    it('should hit when a sphere is behind a ray', () => {
      const ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const points = sphere.intersect(ray);
      expect(points.ts).toEqual([-6.0, -4.0]);
    });

    it('should intersect a scaled sphere', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere(Matrix4x4.scaling(2, 2, 2));
      const points = sphere.intersect(ray);
      expect(points.ts).toEqual([3, 7]);
    });

    it('should intersect a translated sphere', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere(Matrix4x4.translation(5, 0, 0));
      const points = sphere.intersect(ray);
      expect(points.length).toBe(0);
    });
  });
});
