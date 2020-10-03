import { Point, Vector } from '../src/tuples';

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
    // projectile starts one unit above the origin
    // velocity is normalized to 1 unit/tick
    let cannonball = { position: new Point(0, 1, 0), velocity: new Vector(1, 1, 0) };

    // gravity -0.1 unit/tick and wind is -0.01 unit/tick
    const environment = { gravity: new Vector(0, -0.1, 0), wind: new Vector(-0.01, 0, 0) };

    let tickCount = 0;
    while (cannonball.position.y > 0) {
      cannonball = tick(environment, cannonball);
      tickCount++;
    }

    expect(tickCount).toBe(22);
  });
});
