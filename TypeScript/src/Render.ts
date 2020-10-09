import { Camera } from './Camera';
import { Canvas } from './Canvas';
import { World } from './World';

export function render(camera: Camera, world: World): Canvas {
  const canvas = new Canvas(camera.hsize, camera.vsize);

  for (let y = 0; y < camera.vsize; y++) {
    for (let x = 0; x < camera.hsize; x++) {
      const ray = camera.rayForPixel(x, y);
      const color = world.colorAt(ray);
      canvas.setPixel(x, y, color);
    }
  }

  return canvas;
}
