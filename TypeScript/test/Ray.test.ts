import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';

describe('Ray', () => {
  describe('ctor()', () => {
    it('should store an origin and direction', () => {
      const origin = new Point(1, 2, 3);
      const direction = new Vector(4, 5, 6);
      const ray = new Ray(origin, direction);
      expect(ray.origin).toBe(origin);
      expect(ray.direction).toBe(direction);
    });
  });

  describe('position()', () => {
    it('should calculate a point from a distance', () => {
      const ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
      expect(ray.position(0)).toEqual(new Point(2, 3, 4));
      expect(ray.position(1)).toEqual(new Point(3, 3, 4));
      expect(ray.position(-1)).toEqual(new Point(1, 3, 4));
      expect(ray.position(2.5)).toEqual(new Point(4.5, 3, 4));
    });
  });

  describe('transform()', () => {
    it('should translate a ray', () => {
      const ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
      const matrix = Matrix4x4.translation(3, 4, 5);
      const translatedRay = ray.transform(matrix);
      expect(translatedRay.origin).toEqual(new Point(4, 6, 8));
      expect(translatedRay.direction).toEqual(new Vector(0, 1, 0));
    });

    it('should scale a ray', () => {
      const ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
      const matrix = Matrix4x4.scaling(2, 3, 4);
      const translatedRay = ray.transform(matrix);
      expect(translatedRay.origin).toEqual(new Point(2, 6, 12));
      expect(translatedRay.direction).toEqual(new Vector(0, 3, 0));
    });
  });
});
