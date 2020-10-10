import { Color } from '../src/Color';
import { Intersection, IntersectionList } from '../src/Intersections';
import { PointLight } from '../src/Lights';
import { Material } from '../src/Materials';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Ray } from '../src/Ray';
import { Plane, Sphere } from '../src/Shapes';
import { World } from '../src/World';
import { TestPattern } from './Patterns.test';

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
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.shadeHit(comps);
      expect(color.isEqualTo(new Color(0.38066, 0.47583, 0.2855))).toBeTrue();
    });

    it('should shade an intersection from the inside', () => {
      const world = createDefaultWorld().withLight(new PointLight(new Point(0, 0.25, 0), Color.White));
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const shape = world.shapes[1];
      const intersection = new Intersection(0.5, shape);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.shadeHit(comps);
      expect(color.isEqualTo(new Color(0.90498, 0.90498, 0.90498))).toBeTrue();
    });

    it('should shade an intersection in shadow', () => {
      const sphere1 = new Sphere();
      const sphere2 = new Sphere(Matrix4x4.translation(0, 0, 10));
      const world = new World(new PointLight(new Point(0, 0, -10), Color.White), [sphere1, sphere2]);
      const ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
      const intersection = new Intersection(4, sphere2);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.shadeHit(comps);
      expect(color).toEqual(new Color(0.1, 0.1, 0.1));
    });

    // Add a reflective plane to the default scene, just below the spheres, and orient a ray so it
    // strikes the plane, reflects upward, and hits the outermost sphere.
    it('should calculate the shade color for a reflective material', () => {
      const floor = new Plane(Matrix4x4.translation(0, -1, 0), new Material().withReflective(0.5));
      const world = createDefaultWorld().addShape(floor);
      const ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersection = new Intersection(Math.SQRT2, floor);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.shadeHit(comps);
      expect(color.isEqualTo(new Color(0.87675, 0.92434, 0.82917))).toBeTrue();
    });

    // Add a glass floor to the default world, positioned just below the two default spheres, and
    // add a new, colored sphere below the floor. Cast a ray diagonally toward the floor, with the
    // expectation that it will refract and eventually strike the colored ball. Because the plan is
    // only semitransparent, the resulting color should combine the refracted color of the ball and
    // the color of the plane.
    it('should calculate the shade color with a transparent material', () => {
      const floor = new Plane(
        Matrix4x4.translation(0, -1, 0),
        new Material().withTransparency(0.5).withRefractiveIndex(1.5),
      );
      const ball = new Sphere(Matrix4x4.translation(0, -3.5, -0.5), new Material(new Color(1, 0, 0), 0.5));
      const world = createDefaultWorld().addShape(floor).addShape(ball);

      const ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersections = new IntersectionList(new Intersection(Math.SQRT2, floor));
      const comps = intersections.get(0).prepareComputations(ray, intersections);
      const color = world.shadeHit(comps, 5);
      expect(color.isEqualTo(new Color(0.93642, 0.68642, 0.68642))).toBeTrue();
    });

    it('should calculate the shade color with a relective and transparent material', () => {
      const floor = new Plane(
        Matrix4x4.translation(0, -1, 0),
        new Material().withReflective(0.5).withTransparency(0.5).withRefractiveIndex(1.5),
      );
      const ball = new Sphere(Matrix4x4.translation(0, -3.5, -0.5), new Material(new Color(1, 0, 0), 0.5));
      const world = createDefaultWorld().addShape(floor).addShape(ball);

      const ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersections = new IntersectionList(new Intersection(Math.SQRT2, floor));
      const comps = intersections.get(0).prepareComputations(ray, intersections);
      const color = world.shadeHit(comps, 5);
      expect(color.isEqualTo(new Color(0.93391, 0.69643, 0.69243))).toBeTrue();
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

    it('should avoid infinite recursion with two parallel mirrors', () => {
      const lower = new Plane(Matrix4x4.translation(0, -1, 0)).withMaterial(new Material().withReflective(1));
      const upper = new Plane(Matrix4x4.translation(0, 1, 0)).withMaterial(new Material().withReflective(1));
      const world = createDefaultWorld()
        .withLight(new PointLight(new Point(0, 0, 0), Color.White))
        .addShape(lower)
        .addShape(upper);
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
      const color = world.colorAt(ray);
      expect(color).toEqual(new Color(1.9, 1.9, 1.9));
    });
  });

  describe('isShadowed()', () => {
    it('should return false when nothing is colinear with the point and light', () => {
      const world = createDefaultWorld();
      const point = new Point(0, 10, 0);
      expect(world.isShadowed(point)).toBeFalse();
    });

    it('should return true when an object is between the point and the light', () => {
      const world = createDefaultWorld();
      const point = new Point(10, -10, 10);
      expect(world.isShadowed(point)).toBeTrue();
    });

    it('should return false when an object is behind the light', () => {
      const world = createDefaultWorld();
      const point = new Point(-20, 20, -20);
      expect(world.isShadowed(point)).toBeFalse();
    });

    it('should return false when an object is behind the point', () => {
      const world = createDefaultWorld();
      const point = new Point(-2, 2, -2);
      expect(world.isShadowed(point)).toBeFalse();
    });
  });

  describe('reflectedColor()', () => {
    it('should return black for the reflected color of a nonreflective material', () => {
      let world = createDefaultWorld();
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      let shape = world.shapes[1];
      shape = shape.withMaterial(shape.material.withAmbient(1));
      world = world.withShapes([world.shapes[0], shape, ...world.shapes.slice(2)]);
      const intersection = new Intersection(1, shape);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.reflectedColor(comps);
      expect(color).toEqual(Color.Black);
    });

    // Add a reflective plane to the default scene, just below the spheres, and orient a ray so it
    // strikes the plane, reflects upward, and hits the outermost sphere.
    it('should calculate the reflected color for a reflective material', () => {
      const floor = new Plane(Matrix4x4.translation(0, -1, 0), new Material().withReflective(0.5));
      const world = createDefaultWorld().addShape(floor);
      const ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersection = new Intersection(Math.SQRT2, floor);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.reflectedColor(comps);
      expect(color.isEqualTo(new Color(0.19033, 0.23791, 0.14274))).toBeTrue();
    });

    it('should only allow a maximum recursion depth', () => {
      const floor = new Plane(Matrix4x4.translation(0, -1, 0), new Material().withReflective(0.5));
      const world = createDefaultWorld().addShape(floor);
      const ray = new Ray(new Point(0, 0, -3), new Vector(0, -Math.SQRT2 / 2, Math.SQRT2 / 2));
      const intersection = new Intersection(Math.SQRT2, floor);
      const comps = intersection.prepareComputations(ray, new IntersectionList(intersection));
      const color = world.reflectedColor(comps, 0);
      expect(color).toEqual(Color.Black);
    });
  });

  describe('refractedColor()', () => {
    it('should return black for the refracted color with an opaque surface', () => {
      const world = createDefaultWorld();
      const shape = world.shapes[0];
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const intersections = new IntersectionList(new Intersection(4, shape), new Intersection(6, shape));
      const comps = intersections.get(0).prepareComputations(ray, intersections);
      const color = world.refractedColor(comps, 5);
      expect(color).toEqual(Color.Black);
    });

    it('should return the refracted color at the maximum recursive depth', () => {
      let world = createDefaultWorld();
      const shape = world.shapes[0].addToMaterial((m) => m.withTransparency(1.0).withRefractiveIndex(1.5));
      world = world.withShapes([shape, ...world.shapes.slice(1)]);

      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const intersections = new IntersectionList(new Intersection(4, shape), new Intersection(6, shape));
      const comps = intersections.get(0).prepareComputations(ray, intersections);
      const color = world.refractedColor(comps, 0);
      expect(color).toEqual(Color.Black);
    });

    it('should return black for the refracted color under total internal reflection', () => {
      let world = createDefaultWorld();
      const shape = world.shapes[0].addToMaterial((m) => m.withTransparency(1.0).withRefractiveIndex(1.5));
      world = world.withShapes([shape, ...world.shapes.slice(1)]);

      const ray = new Ray(new Point(0, 0, Math.SQRT2 / 2), new Vector(0, 1, 0));
      const intersections = new IntersectionList(
        new Intersection(-Math.SQRT2 / 2, shape),
        new Intersection(Math.SQRT2 / 2, shape),
      );

      // Note this time we're inside the sphere, so we need to look at intersections[1], not 0.
      const comps = intersections.get(1).prepareComputations(ray, intersections);
      const color = world.refractedColor(comps, 5);
      expect(color).toEqual(Color.Black);
    });

    it('should return the refracted color with a refracted ray', () => {
      let world = createDefaultWorld();
      const a = world.shapes[0]
        .addToMaterial((m) => m.withAmbient(1.0))
        .addToMaterial((m) => m.withPattern(new TestPattern()));
      const b = world.shapes[1].addToMaterial((m) => m.withTransparency(1.0).withRefractiveIndex(1.5));
      world = world.withShapes([a, b, ...world.shapes.slice(2)]);

      const ray = new Ray(new Point(0, 0, 0.1), new Vector(0, 1, 0));
      const intersections = new IntersectionList(
        new Intersection(-0.9899, a),
        new Intersection(-0.4899, b),
        new Intersection(0.4899, b),
        new Intersection(0.9899, a),
      );
      const comps = intersections.get(2).prepareComputations(ray, intersections);
      const color = world.refractedColor(comps, 5);
      expect(color.isEqualTo(new Color(0, 0.99887, 0.04722))).toBeTrue();
    });
  });
});

export function createDefaultWorld(): World {
  const light = new PointLight(new Point(-10, 10, -10), Color.White);
  const sphere1 = new Sphere(undefined, new Material(new Color(0.8, 1.0, 0.6), undefined, 0.7, 0.2));
  const sphere2 = new Sphere(Matrix4x4.scaling(0.5, 0.5, 0.5));
  return new World(light, [sphere1, sphere2]);
}
