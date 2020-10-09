import { Intersection, IntersectionList } from '../src/Intersections';
import { EPSILON } from '../src/Math';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';
import { Sphere, Plane } from '../src/Shapes';
import { createGlassSphere } from './Shapes.test';

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
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
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
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      expect(comps.isInside).toBeFalse();
    });

    it('should calculate the hit when an intersection occurs on the inside', () => {
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const sphere = new Sphere();
      const intersection = new Intersection(1, sphere);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      expect(comps.point).toEqual(new Point(0, 0, 1));
      expect(comps.eye.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
      expect(comps.isInside).toBeTrue();

      // The normal would have been (0, 0, 1) but it's inverted!
      expect(comps.normal.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
    });

    it('the hit should offset the point', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const sphere = new Sphere(Matrix4x4.translation(0, 0, 1));
      const intersection = new Intersection(5, sphere);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      expect(comps.overPoint.z).toBeLessThan(-EPSILON / 2);
      expect(comps.point.z).toBeGreaterThan(comps.overPoint.z);
    });

    it('should precompute the reflection vector', () => {
      const shape = new Plane();
      const ray = new Ray(new Point(0, 1, -1), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersection = new Intersection(Math.SQRT2, shape);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      expect(comps.reflect).toEqual(new Vector(0, Math.SQRT2 / 2, Math.SQRT2 / 2));
    });

    describe('should find n1 and n2 at various intersections', () => {
      // We'll set up a scene with three glass spheres, A, B, and C. Sphere A contains both B and C
      // completely and B is to the left of C and overlaps. We shoot a ray through the middle, which
      // will have 6 intersections and calculate the n1 and n2 values for each intersection.

      interface TestCase {
        readonly index: number;
        readonly n1: number;
        readonly n2: number;
      }

      const testCases = [
        { index: 0, n1: 1.0, n2: 1.5 },
        { index: 1, n1: 1.5, n2: 2.0 },
        { index: 2, n1: 2.0, n2: 2.5 },
        { index: 3, n1: 2.5, n2: 2.5 },
        { index: 4, n1: 2.5, n2: 1.5 },
        { index: 5, n1: 1.5, n2: 1.0 },
      ];

      function test(testCase: TestCase): void {
        const a = createGlassSphere()
          .withTransform(Matrix4x4.scaling(2, 2, 2))
          .addToMaterial((m) => m.withRefractiveIndex(1.5));

        const b = createGlassSphere()
          .withTransform(Matrix4x4.translation(0, 0, -0.25))
          .addToMaterial((m) => m.withRefractiveIndex(2.0));

        const c = createGlassSphere()
          .withTransform(Matrix4x4.translation(0, 0, 0.25))
          .addToMaterial((m) => m.withRefractiveIndex(2.5));

        const ray = new Ray(new Point(0, 0, -4), new Vector(0, 0, 1));
        const intersections = new IntersectionList(
          new Intersection(2, a),
          new Intersection(2.75, b),
          new Intersection(3.25, c),
          new Intersection(4.75, b),
          new Intersection(5.25, c),
          new Intersection(6, a),
        );

        const comps = intersections.get(testCase.index).prepareComputations(ray, intersections);
        expect(comps.n1).toEqual(testCase.n1, `for test case ${testCase.index} for n1`);
        expect(comps.n2).toEqual(testCase.n2, `for test case ${testCase.index} for n2`);
      }

      testCases.forEach((testCase) => {
        it('should run each test', () => {
          test(testCase);
        });
      });
    });

    it('should set the underPoint to just below the surface', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const shape = createGlassSphere().withTransform(Matrix4x4.translation(0, 0, 1));
      const intersection = new Intersection(5, shape);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      expect(comps.underPoint.z).toBeGreaterThan(EPSILON / 2);
      expect(comps.point.z).toBeLessThan(comps.underPoint.z);
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
