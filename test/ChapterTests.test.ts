import { Canvas } from '../src/Canvas';
import { Color } from '../src/Color';
import { Point, Vector } from '../src/PointVector';
import { Rect } from '../src/Rect';
import * as path from 'path';

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

describe('Firing a cannon', () => {
  it('should update the cannonball after each tick', () => {
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
      const rect = new Rect(point.y - borderSize, point.x - borderSize, point.y + borderSize, point.x + borderSize);
      canvas.fillRect(rect, color);
    }

    const fileName = path.resolve(__dirname, '..', 'test-output', 'cannon.ppm');
    canvas.saveToPpmFile(fileName);
  });
});
