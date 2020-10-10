import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';
import { Ray } from './Ray';

export class Camera {
  public readonly hsize: number;
  public readonly vsize: number;
  public readonly fieldOfView: number;
  public readonly transform: Matrix4x4;
  public readonly pixelSize: number;

  private readonly halfWidth: number;
  private readonly halfHeight: number;

  public constructor(hsize: number, vsize: number, fieldOfView: number, transform = Matrix4x4.identity) {
    this.hsize = hsize;
    this.vsize = vsize;
    this.fieldOfView = fieldOfView;
    this.transform = transform;

    // Calculate the pixel size and store some other calculations for later.
    const halfView = Math.tan(fieldOfView / 2);
    const aspect = hsize / vsize;

    if (aspect >= 1) {
      this.halfWidth = halfView;
      this.halfHeight = halfView / aspect;
    } else {
      this.halfWidth = halfView * aspect;
      this.halfHeight = halfView;
    }

    this.pixelSize = (this.halfWidth * 2) / hsize;
  }

  public rayForPixel(x: number, y: number): Ray {
    // The offset from the edge of the canvas to the pixel's center.
    const offsetX: number = (x + 0.5) * this.pixelSize;
    const offsetY: number = (y + 0.5) * this.pixelSize;

    // The untransformed coordinates of the pixel in world space. (Remember that the camera looks
    // towards -z, so +x is to the *left*).
    const worldX: number = this.halfWidth - offsetX;
    const worldY: number = this.halfHeight - offsetY;

    // Using the camera matrix, transform the canvas point and the origin, and then compute the
    // ray's direction vector. (Remember that the canvas is at z=-1).
    const pixel: Point = this.transform.inverse().multiplyByPoint(new Point(worldX, worldY, -1));
    const origin: Point = this.transform.inverse().multiplyByPoint(new Point(0, 0, 0));
    const direction: Vector = pixel.subtract(origin).normalize();

    return new Ray(origin, direction);
  }
}
