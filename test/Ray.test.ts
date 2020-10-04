import { Point, Vector } from '../src/PointVector';
import { hit, Intersection, IntersectionList, Ray } from '../src/Ray';
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
      const list = new IntersectionList(new Intersection(30, sphere), new Intersection(-1, sphere));
      list.add(new Intersection(10, sphere), new Intersection(-300, sphere));
      expect(list.length).toBe(4);
      expect(list.values.map((x) => x.t)).toEqual([-300, -1, 10, 30]);
    });
  });
});

describe('hit()', () => {
  it('should return the smallest t when all intersections have positive t', () => {
    const sphere = new Sphere();
    const i1 = new Intersection(1, sphere);
    const i2 = new Intersection(2, sphere);
    const list = new IntersectionList(i1, i2);
    expect(hit(list)).toBe(i1);
  });

  it('should return the smallest non-negative t when some intersections have negative t', () => {
    const sphere = new Sphere();
    const i1 = new Intersection(-1, sphere);
    const i2 = new Intersection(1, sphere);
    const list = new IntersectionList(i1, i2);
    expect(hit(list)).toBe(i2);
  });

  it('should return null when all intersections have negative t', () => {
    const sphere = new Sphere();
    const i1 = new Intersection(-2, sphere);
    const i2 = new Intersection(-1, sphere);
    const list = new IntersectionList(i1, i2);
    expect(hit(list)).toBeNull();
  });

  it('should return the lowest non-negative intersection', () => {
    const sphere = new Sphere();
    const i1 = new Intersection(5, sphere);
    const i2 = new Intersection(7, sphere);
    const i3 = new Intersection(-3, sphere);
    const i4 = new Intersection(2, sphere);
    const list = new IntersectionList(i1, i2, i3, i4);
    expect(hit(list)).toBe(i4);
  });
});
