import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { viewTransform } from '../src/Transformations';

describe('viewTransform()', () => {
  it('should use the identity matrix for the default orientation', () => {
    const from = new Point(0, 0, 0);
    const to = new Point(0, 0, -1);
    const up = new Vector(0, 1, 0);
    const transform = viewTransform(from, to, up);
    expect(transform).toEqual(Matrix4x4.identity);
  });

  it('should reflect the scene looking in the positive z direction', () => {
    const from = new Point(0, 0, 0);
    const to = new Point(0, 0, 1);
    const up = new Vector(0, 1, 0);
    const transform = viewTransform(from, to, up);
    expect(transform).toEqual(Matrix4x4.scaling(-1, 1, -1));
  });

  it('should move the world and not the eye', () => {
    const from = new Point(0, 0, 8);
    const to = new Point(0, 0, 0);
    const up = new Vector(0, 1, 0);
    const transform = viewTransform(from, to, up);
    expect(transform).toEqual(Matrix4x4.translation(0, 0, -8));
  });

  it('should move the world using an arbitrary view transformation', () => {
    const from = new Point(1, 3, 2);
    const to = new Point(4, -2, 8);
    const up = new Vector(1, 1, 0);
    const transform = viewTransform(from, to, up);

    // prettier-ignore
    const expected = new Matrix4x4(
      -0.50709, 0.50709,  0.67612, -2.36643,
       0.76772, 0.60609,  0.12122, -2.82843,
      -0.35857, 0.59761, -0.71714,  0.00000,
       0.00000, 0.00000,  0.00000,  1.00000);

    expect(transform.isEqualTo(expected)).toBeTrue();
  });
});
