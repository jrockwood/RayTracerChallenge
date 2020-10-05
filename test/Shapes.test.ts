import { Material } from '../src/Materials';
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

    it('should have a default material', () => {
      const sphere = new Sphere();
      expect(sphere.material).toEqual(new Material());
    });

    it('should store the material', () => {
      const sphere = new Sphere(undefined, new Material(undefined, 1));
      expect(sphere.material.ambient).toBe(1);
    });
  });

  describe('intersect()', () => {
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

  describe('normalAt()', () => {
    it('should return the normal on a sphere at a point on the x axis', () => {
      const sphere = new Sphere();
      const normal = sphere.normalAt(new Point(1, 0, 0));
      expect(normal).toEqual(new Vector(1, 0, 0));
    });

    it('should return the normal on a sphere at a point on the y axis', () => {
      const sphere = new Sphere();
      const normal = sphere.normalAt(new Point(0, 1, 0));
      expect(normal).toEqual(new Vector(0, 1, 0));
    });

    it('should return the normal on a sphere at a point on the z axis', () => {
      const sphere = new Sphere();
      const normal = sphere.normalAt(new Point(0, 0, 1));
      expect(normal).toEqual(new Vector(0, 0, 1));
    });

    it('should return the normal on a sphere at a nonaxial point', () => {
      const sphere = new Sphere();
      const sqrt3Over3 = Math.sqrt(3) / 3;
      const normal = sphere.normalAt(new Point(sqrt3Over3, sqrt3Over3, sqrt3Over3));
      expect(normal).toEqual(new Vector(sqrt3Over3, sqrt3Over3, sqrt3Over3));
    });

    it('should return a normalized vector', () => {
      const sphere = new Sphere();
      const sqrt3Over3 = Math.sqrt(3) / 3;
      const normal = sphere.normalAt(new Point(sqrt3Over3, sqrt3Over3, sqrt3Over3));
      expect(normal).toEqual(normal.normalize());
    });

    it('should calculate the normal on a translated sphere', () => {
      const sphere = new Sphere(Matrix4x4.translation(0, 1, 0));
      const normal = sphere.normalAt(new Point(0, 1.70711, -0.70711));
      expect(normal.isEqualTo(new Vector(0, 0.70711, -0.70711))).toBeTrue();
    });

    it('should calculate the normal on a transformed sphere', () => {
      const sphere = new Sphere(Matrix4x4.rotationZ(Math.PI / 5).scale(1, 0.5, 1));
      const normal = sphere.normalAt(new Point(0, Math.SQRT2 / 2, -Math.SQRT2 / 2));
      expect(normal.isEqualTo(new Vector(0, 0.97014, -0.24254))).toBeTrue();
    });
  });
});
