import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Intersection, IntersectionList, Ray } from '../src/Ray';
import { Sphere } from '../src/Shapes';

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

describe('Intersection', () => {
  describe('ctor()', () => {
    it('should store t and a shape', () => {
      const sphere = new Sphere();
      const intersection = new Intersection(3.5, sphere);
      expect(intersection.t).toBe(3.5);
      expect(intersection.shape).toBe(sphere);
    });
  });

  describe('prepareComputations()', () => {
    it('should store the precalculated results of an intersection', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const intersection = new Intersection(4, sphere);
      const comps = intersection.prepareComputations(ray);
      expect(comps.t).toBe(4);
      expect(comps.shape).toBe(sphere);
      expect(comps.point).toEqual(new Point(0, 0, -1));
      expect(comps.eye.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
      expect(comps.normal).toEqual(new Vector(0, 0, -1));
    });

    it('should calculate the hit when an intersection occurs on the outside', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const intersection = new Intersection(4, sphere);
      const comps = intersection.prepareComputations(ray);
      expect(comps.isInside).toBeFalse();
    });

    it('should calculate the hit when an intersection occurs on the inside', () => {
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const intersection = new Intersection(1, sphere);
      const comps = intersection.prepareComputations(ray);
      expect(comps.point).toEqual(new Point(0, 0, 1));
      expect(comps.eye.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
      expect(comps.isInside).toBeTrue();

      // The normal would have been (0, 0, 1) but it's inverted!
      expect(comps.normal.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
    });
  });
});

describe('IntersectionList', () => {
  describe('ctor()', () => {
    it('should store the intersections in sorted order', () => {
      const sphere = new Sphere();
      const list = new IntersectionList(new Intersection(30, sphere), new Intersection(-1, sphere));
      expect(list.length).toBe(2);
      expect(list.values.map((x) => x.t)).toEqual([-1, 30]);
    });
  });

  describe('get()', () => {
    it('should get the elements', () => {
      const sphere = new Sphere();
      const list = new IntersectionList(new Intersection(30, sphere), new Intersection(-1, sphere));
      expect(list.get(0).t).toBe(-1);
      expect(list.get(1).t).toBe(30);
    });

    it('should throw if the index is out of bounds', () => {
      const list = new IntersectionList();
      expect(() => list.get(0)).toThrow();
    });
  });

  describe('add()', () => {
    it('should add to the existing list in sorted order', () => {
      const sphere = new Sphere();
      let list = new IntersectionList(new Intersection(30, sphere), new Intersection(-1, sphere));
      list = list.add(new Intersection(10, sphere), new Intersection(-300, sphere));
      expect(list.length).toBe(4);
      expect(list.values.map((x) => x.t)).toEqual([-300, -1, 10, 30]);
    });
  });

  describe('hit()', () => {
    it('should return the smallest t when all intersections have positive t', () => {
      const sphere = new Sphere();
      const i1 = new Intersection(1, sphere);
      const i2 = new Intersection(2, sphere);
      const list = new IntersectionList(i1, i2);
      expect(list.hit()).toBe(i1);
    });

    it('should return the smallest non-negative t when some intersections have negative t', () => {
      const sphere = new Sphere();
      const i1 = new Intersection(-1, sphere);
      const i2 = new Intersection(1, sphere);
      const list = new IntersectionList(i1, i2);
      expect(list.hit()).toBe(i2);
    });

    it('should return null when all intersections have negative t', () => {
      const sphere = new Sphere();
      const i1 = new Intersection(-2, sphere);
      const i2 = new Intersection(-1, sphere);
      const list = new IntersectionList(i1, i2);
      expect(list.hit()).toBeNull();
    });

    it('should return the lowest non-negative intersection', () => {
      const sphere = new Sphere();
      const i1 = new Intersection(5, sphere);
      const i2 = new Intersection(7, sphere);
      const i3 = new Intersection(-3, sphere);
      const i4 = new Intersection(2, sphere);
      const list = new IntersectionList(i1, i2, i3, i4);
      expect(list.hit()).toBe(i4);
    });
  });
});
