// file.skip
// Tell Wallaby to skip this file

import { Canvas } from '../src/Canvas';
import { Color } from '../src/Color';
import { Point, Vector } from '../src/PointVector';
import { Rect } from '../src/Rect';
import * as path from 'path';
import { Matrix4x4 } from '../src/Matrices';

interface Projectile {
  position: Point;
  velocity: Vector;
}

interface Environment {
  gravity: Vector;
  wind: Vector;
}

function tick(environment: Environment, projectile: Projectile): Projectile {
  const position = projectile.position.add(projectile.velocity);
  const velocity = projectile.velocity.add(environment.gravity).add(environment.wind);
  return { position, velocity };
}

function makeRect(x: number, y: number, borderSize: number): Rect {
  const rect = new Rect(y - borderSize, x - borderSize, y + borderSize, x + borderSize);
  return rect;
}

function saveCanvas(canvas: Canvas, fileName: string): void {
  const resolvedPath = path.resolve(__dirname, '..', 'test-output', fileName);
  canvas.saveToPpmFile(resolvedPath);
}

describe('Chapter Tests', () => {
  it('Firing a cannon', () => {
    const start = new Point(0, 1, 0);
    const velocity = new Vector(1, 1.8, 0).normalize().multiply(11.25);
    let cannonball = { position: start, velocity };

    // gravity -0.1 unit/tick and wind is -0.01 unit/tick
    const gravity = new Vector(0, -0.1, 0);
    const wind = new Vector(-0.01, 0, 0);
    const environment = { gravity, wind };

    const color = Color.White;
    const borderSize = 2;
    const canvas = new Canvas(900, 550);

    while (cannonball.position.y > 0) {
      cannonball = tick(environment, cannonball);
      const point = new Point(cannonball.position.x, canvas.height - cannonball.position.y, 0).roundToInt();
      const rect = makeRect(point.x, point.y, borderSize);
      canvas.fillRect(rect, color);
    }

    saveCanvas(canvas, 'cannon.ppm');
  });

  it('Draw a clock face', () => {
    const canvas = new Canvas(800, 800);
    const centerX = Math.round(canvas.width / 2);
    const centerY = Math.round(canvas.height / 2);
    const clockRadius = Math.min(canvas.width * (3 / 8), canvas.height * (3 / 8));

    const color = Color.White;
    const borderSize = 4;

    // The clock is oriented along the y axis, which means when looking at it face-on you're looking
    // towards the negative y axis and the clock face sits on the x-z plane.
    const twelve = new Point(0, 0, 1);
    const rotationAngle = (2 * Math.PI) / 12;

    for (let hour = 0; hour < 12; hour++) {
      // Rotate the twelve point around the y axis.
      // Scale by the clock radius.
      // Translate to the center of the canvas.
      const transform = Matrix4x4.rotationY(hour * rotationAngle);
      const hourPoint = transform.multiplyByPoint(twelve);
      const x = Math.round(centerX + hourPoint.x * clockRadius);
      const y = Math.round(centerY - hourPoint.z * clockRadius);
      const rect = makeRect(x, y, borderSize);
      canvas.fillRect(rect, color);
    }

    saveCanvas(canvas, 'clock-face.ppm');
  });
});
