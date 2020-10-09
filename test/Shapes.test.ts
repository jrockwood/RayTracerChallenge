import { Material } from '../src/Materials';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { Intersection, IntersectionList, Ray } from '../src/Ray';
import { Plane, Shape, Sphere } from '../src/Shapes';

class TestShape extends Shape {
  public savedLocalRay?: Ray;
  public savedLocalPoint?: Point;

  public constructor(transform?: Matrix4x4, material?: Material) {
    super(transform, material);
  }

  protected localIntersect(localRay: Ray): IntersectionList {
    this.savedLocalRay = localRay;
    return new IntersectionList();
  }

  protected localNormalAt(localPoint: Point): Vector {
    this.savedLocalPoint = localPoint;
    return new Vector(localPoint.x, localPoint.y, localPoint.z);
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public withTransform(value: Matrix4x4): Shape {
    throw new Error('Method not implemented.');
  }
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  public withMaterial(value: Material): Shape {
    throw new Error('Method not implemented.');
  }
}

export function createGlassSphere(): Sphere {
  return new Sphere(undefined, new Material().withTransparency(1.0).withRefractiveIndex(1.5));
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Shape

describe('Shape', () => {
  describe('ctor()', () => {
    it('should default to the identity matrix for the transform', () => {
      const shape = new TestShape();
      expect(shape.transform).toEqual(Matrix4x4.identity);
    });

    it('should store the transform', () => {
      const shape = new TestShape(Matrix4x4.translation(2, 3, 4));
      expect(shape.transform).toEqual(Matrix4x4.translation(2, 3, 4));
    });

    it('should have a default material', () => {
      const shape = new TestShape();
      expect(shape.material).toEqual(new Material());
    });

    it('should store the material', () => {
      const shape = new TestShape(undefined, new Material(undefined, 1));
      expect(shape.material.ambient).toBe(1);
    });
  });

  describe('intersect()', () => {
    it('should intersect a scaled shape', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const shape = new TestShape(Matrix4x4.scaling(2, 2, 2));
      shape.intersect(ray);
      expect(shape.savedLocalRay?.origin).toEqual(new Point(0, 0, -2.5));
      expect(shape.savedLocalRay?.direction).toEqual(new Vector(0, 0, 0.5));
    });

    it('should intersect a translated sphere', () => {
      const ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
      const shape = new TestShape(Matrix4x4.translation(5, 0, 0));
      shape.intersect(ray);
      expect(shape.savedLocalRay?.origin).toEqual(new Point(-5, 0, -5));
      expect(shape.savedLocalRay?.direction).toEqual(new Vector(0, 0, 1));
    });
  });

  describe('normalAt()', () => {
    it('should calculate the normal on a translated shape', () => {
      const shape = new TestShape(Matrix4x4.translation(0, 1, 0));
      const normal = shape.normalAt(new Point(0, 1.70711, -0.70711));
      expect(normal.isEqualTo(new Vector(0, 0.70711, -0.70711))).toBeTrue();
    });

    it('should calculate the normal on a transformed shape', () => {
      const shape = new TestShape(Matrix4x4.rotationZ(Math.PI / 5).scale(1, 0.5, 1));
      const normal = shape.normalAt(new Point(0, Math.SQRT2 / 2, -Math.SQRT2 / 2));
      expect(normal.isEqualTo(new Vector(0, 0.97014, -0.24254))).toBeTrue();
    });
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sphere

describe('Sphere', () => {
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
  });
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Plane

describe('Plane', () => {
  describe('localNormalAt()', () => {
    it('should return the same normal at every point', () => {
      const plane = new Plane();
      const expectedNormal = new Vector(0, 1, 0);
      expect(plane.normalAt(new Point(0, 0, 0))).toEqual(expectedNormal);
      expect(plane.normalAt(new Point(10, 0, -10))).toEqual(expectedNormal);
      expect(plane.normalAt(new Point(-5, 0, 150))).toEqual(expectedNormal);
    });
  });

  describe('localIntersect()', () => {
    it('should not intersect with a ray parallel to the plane', () => {
      const plane = new Plane();
      const ray = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));
      const intersections = plane.intersect(ray);
      expect(intersections.length).toBe(0);
    });

    it('should not intersect with a coplanar ray', () => {
      const plane = new Plane();
      const ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
      const intersections = plane.intersect(ray);
      expect(intersections.length).toBe(0);
    });

    it('should intersect with a ray from above', () => {
      const plane = new Plane();
      const ray = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));
      const intersections = plane.intersect(ray);
      expect(intersections.values).toEqual([new Intersection(1, plane)]);
    });

    it('should intersect with a ray from below', () => {
      const plane = new Plane();
      const ray = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));
      const intersections = plane.intersect(ray);
      expect(intersections.values).toEqual([new Intersection(1, plane)]);
    });
  });
});
