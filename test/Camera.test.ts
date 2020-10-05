import { Camera } from '../src/Camera';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';

describe('Camera', () => {
  describe('ctor()', () => {
    it('should store the input parameters', () => {
      const hsize = 160;
      const vsize = 120;
      const fieldOfView = Math.PI / 2;
      const camera = new Camera(hsize, vsize, fieldOfView);
      expect(camera.hsize).toBe(hsize);
      expect(camera.vsize).toBe(vsize);
      expect(camera.fieldOfView).toBe(fieldOfView);
      expect(camera.transform).toEqual(Matrix4x4.identity);
    });
  });

  describe('pixelSize', () => {
    it('should calculate correctly for a horizontal canvas', () => {
      const camera = new Camera(200, 125, Math.PI / 2);
      expect(camera.pixelSize).toBeCloseTo(0.01);
    });

    it('should calculate correctly for a vertical canvas', () => {
      const camera = new Camera(125, 200, Math.PI / 2);
      expect(camera.pixelSize).toBeCloseTo(0.01);
    });
  });

  describe('rayForPixel()', () => {
    it('should construct a ray through the center of the canvas', () => {
      const camera = new Camera(201, 101, Math.PI / 2);
      const ray = camera.rayForPixel(100, 50);
      expect(ray.origin).toEqual(new Point(0, 0, 0));
      expect(ray.direction.isEqualTo(new Vector(0, 0, -1))).toBeTrue();
    });

    it('should construct a ray through a corner of the canvas', () => {
      const camera = new Camera(201, 101, Math.PI / 2);
      const ray = camera.rayForPixel(0, 0);
      expect(ray.origin).toEqual(new Point(0, 0, 0));
      expect(ray.direction.isEqualTo(new Vector(0.66519, 0.33259, -0.66851))).toBeTrue();
    });

    it('should construct a ray when the camera is transformed', () => {
      const camera = new Camera(
        201,
        101,
        Math.PI / 2,
        Matrix4x4.rotationY(Math.PI / 4).multiply(Matrix4x4.translation(0, -2, 5)),
      );
      const ray = camera.rayForPixel(100, 50);
      expect(ray.origin).toEqual(new Point(0, 2, -5));
      expect(ray.direction.isEqualTo(new Vector(Math.SQRT2 / 2, 0, -Math.SQRT2 / 2))).toBeTrue();
    });
  });
});
