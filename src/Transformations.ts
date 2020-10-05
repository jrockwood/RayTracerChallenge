import { Matrix4x4 } from './Matrices';
import { Point, Vector } from './PointVector';

export function viewTransform(from: Point, to: Point, up: Vector): Matrix4x4 {
  const forward = to.subtract(from).normalize();
  const upNormalized = up.normalize();
  const left = forward.cross(upNormalized);
  const trueUp = left.cross(forward);

  // prettier-ignore
  const orientation = new Matrix4x4(
     left.x,     left.y,     left.z,    0,
     trueUp.x,   trueUp.y,   trueUp.z,  0,
    -forward.x, -forward.y, -forward.z, 0,
     0,          0,          0,         1);

  const result = orientation.multiply(Matrix4x4.translation(-from.x, -from.y, -from.z));
  return result;
}
