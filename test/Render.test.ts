import { Camera } from '../src/Camera';
import { Color } from '../src/Color';
import { Point, Vector } from '../src/PointVector';
import { render } from '../src/Render';
import { viewTransform } from '../src/Transformations';
import { createDefaultWorld } from './World.test';

describe('render()', () => {
  it('should render a world with a camera', () => {
    const world = createDefaultWorld();
    const from = new Point(0, 0, -5);
    const to = new Point(0, 0, 0);
    const up = new Vector(0, 1, 0);
    const cameraTransform = viewTransform(from, to, up);
    const camera = new Camera(11, 11, Math.PI / 2, cameraTransform);
    const canvas = render(camera, world);
    expect(canvas.getPixel(5, 5).isEqualTo(new Color(0.38066, 0.47583, 0.2855))).toBeTrue();
  });
});
