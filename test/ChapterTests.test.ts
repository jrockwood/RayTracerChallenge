// file.skip
// Tell Wallaby to skip this file

import * as path from 'path';
import { Camera } from '../src/Camera';
import { Canvas } from '../src/Canvas';
import { Color } from '../src/Color';
import { Material } from '../src/Materials';
import { Matrix4x4 } from '../src/Matrices';
import { Point, Vector } from '../src/PointVector';
import { PointLight } from '../src/Lights';
import { Ray } from '../src/Ray';
import { Rect } from '../src/Rect';
import { render } from '../src/Render';
import { Plane, Sphere } from '../src/Shapes';
import { viewTransform } from '../src/Transformations';
import { World } from '../src/World';
import { CheckerPattern, StripePattern } from '../src/Patterns';
import { createGlassSphere } from './Shapes.test';
import { Transform } from 'stream';

interface Projectile {
  position: Point;
  velocity: Vector;
}

interface Environment {
  gravity: Vector;
  wind: Vector;
}

function makeRect(x: number, y: number, borderSize: number): Rect {
  const rect = new Rect(y - borderSize, x - borderSize, y + borderSize, x + borderSize);
  return rect;
}

function saveCanvas(canvas: Canvas, fileName: string): void {
  const resolvedPath = path.resolve(__dirname, '..', 'test-output', fileName);
  canvas.saveToPpmFile(resolvedPath);
}

xdescribe('Previous chapter Tests', () => {
  it('Chapter 1-2 - Firing a cannon', () => {
    function tick(environment: Environment, projectile: Projectile): Projectile {
      const position = projectile.position.add(projectile.velocity);
      const velocity = projectile.velocity.add(environment.gravity).add(environment.wind);
      return { position, velocity };
    }

    const start = new Point(0, 1, 0);
    const velocity = new Vector(1, 1.8, 0).normalize().multiply(11.25);
    let cannonball = { position: start, velocity };

    // gravity -0.1 unit/tick and wind is -0.01 unit/tick
    const gravity = new Vector(0, -0.1, 0);
    const wind = new Vector(-0.01, 0, 0);
    const environment = { gravity, wind };

    const color = Color.Magenta;
    const borderSize = 2;
    const canvas = new Canvas(900, 550);

    while (cannonball.position.y > 0) {
      cannonball = tick(environment, cannonball);
      const point = new Point(cannonball.position.x, canvas.height - cannonball.position.y, 0).roundToInt();
      const rect = makeRect(point.x, point.y, borderSize);
      canvas.fillRect(rect, color);
    }

    saveCanvas(canvas, 'ch02-cannon.ppm');
  });

  it('Chapter 4 - Draw a clock face', () => {
    const canvas = new Canvas(400, 400);
    const centerX = Math.round(canvas.width / 2);
    const centerY = Math.round(canvas.height / 2);
    const clockRadius = Math.min(canvas.width * (3 / 8), canvas.height * (3 / 8));

    const color = Color.Cyan;
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

    saveCanvas(canvas, 'ch04-clock-face.ppm');
  });

  it('Chapter 5 - Render a red sphere', () => {
    const canvas = new Canvas(400, 400);
    const color = Color.Red;
    const sphere = new Sphere();

    const rayOrigin = new Point(0, 0, -5);
    const wallZ = 10;
    const wallSize = 7.0;

    const pixelSize = wallSize / canvas.width;
    const halfWallSize = wallSize / 2;

    // For each row of pixels in the canvas...
    for (let y = 0; y < canvas.height; y++) {
      // Compute the world y coordinate (top = +half, bottom = -half).
      const worldY = halfWallSize - pixelSize * y;

      // For each pixel in the row...
      for (let x = 0; x < canvas.width; x++) {
        // Compute the world x coordinate (left = -half, right = +half).
        const worldX = -halfWallSize + pixelSize * x;

        // Describe the point on the wall that the ray will target.
        const position = new Point(worldX, worldY, wallZ);

        // Cast the ray into the scene to see what it hits.
        const ray = new Ray(rayOrigin, position.subtract(rayOrigin).normalize());
        const intersections = sphere.intersect(ray);

        if (intersections.hit()) {
          canvas.setPixel(x, y, color);
        }
      }
    }

    saveCanvas(canvas, 'ch05-red-sphere.ppm');
  });

  it('Chapter 6 - Fully shaded sphere', () => {
    const canvas = new Canvas(200, 200);
    const sphere = new Sphere(undefined, new Material(new Color(1, 0.2, 1)));
    const lightPosition = new Point(-10, 10, -10);
    const lightColor = Color.White;
    const light = new PointLight(lightPosition, lightColor);

    const rayOrigin = new Point(0, 0, -5);
    const wallZ = 10;
    const wallSize = 7.0;

    const pixelSize = wallSize / canvas.width;
    const halfWallSize = wallSize / 2;

    // For each row of pixels in the canvas...
    for (let y = 0; y < canvas.height; y++) {
      // Compute the world y coordinate (top = +half, bottom = -half).
      const worldY = halfWallSize - pixelSize * y;

      // For each pixel in the row...
      for (let x = 0; x < canvas.width; x++) {
        // Compute the world x coordinate (left = -half, right = +half).
        const worldX = -halfWallSize + pixelSize * x;

        // Describe the point on the wall that the ray will target.
        const position = new Point(worldX, worldY, wallZ);

        // Cast the ray into the scene to see what it hits.
        const ray = new Ray(rayOrigin, position.subtract(rayOrigin).normalize());
        const intersections = sphere.intersect(ray);
        const hit = intersections.hit();

        if (hit) {
          const point = ray.position(hit.t);
          const normal = hit.shape.normalAt(point);
          const eye = ray.direction.negate();
          const color = hit.shape.material.lighting(hit.shape, light, point, eye, normal, false);
          canvas.setPixel(x, y, color);
        }
      }
    }

    saveCanvas(canvas, 'ch06-shaded-sphere.ppm');
  });

  it('Chapter 7 - Six Spheres', () => {
    const floor = new Sphere(Matrix4x4.scaling(10, 0.01, 10), new Material(new Color(1, 0.9, 0.9)).withSpecular(0));

    const leftWall = new Sphere(
      Matrix4x4.scaling(10, 0.01, 10)
        .rotateX(Math.PI / 2)
        .rotateY(-Math.PI / 4)
        .translate(0, 0, 5),
      floor.material,
    );

    const rightWall = new Sphere(
      Matrix4x4.scaling(10, 0.01, 10)
        .rotateX(Math.PI / 2)
        .rotateY(Math.PI / 4)
        .translate(0, 0, 5),
      floor.material,
    );

    const middle = new Sphere(
      Matrix4x4.translation(-0.5, 1, 0.5),
      new Material(new Color(0.1, 1, 0.5), undefined, 0.7, 0.3),
    );

    const right = new Sphere(
      Matrix4x4.scaling(0.5, 0.5, 0.5).translate(1.5, 0.5, -0.5),
      new Material(new Color(0.5, 1, 0.1), undefined, 0.7, 0.3),
    );

    const left = new Sphere(
      Matrix4x4.scaling(0.33, 0.33, 0.33).translate(-1.5, 0.33, -0.75),
      new Material(new Color(1, 0.8, 0.1), undefined, 0.7, 0.3),
    );

    const light = new PointLight(new Point(-10, 10, -10), Color.White);
    const world = new World(light, [floor, leftWall, rightWall, middle, right, left]);

    const cameraTransform = viewTransform(new Point(0, 1.5, -5), new Point(0, 1, 0), new Vector(0, 1, 0));
    const camera = new Camera(100, 50, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);

    saveCanvas(canvas, 'ch07-six-spheres.ppm');
  });

  it('Chapter 8 - Shadows', () => {
    // Same as last chapter, but with shadows now!
  });

  it('Chapter 9 - Planes', () => {
    const floor = new Plane(undefined, new Material(new Color(1, 0.9, 0.9)).withSpecular(0));

    const middle = new Sphere(
      Matrix4x4.translation(-0.5, 1, 0.5),
      new Material(new Color(0.1, 1, 0.5), undefined, 0.7, 0.3),
    );

    const right = new Sphere(
      Matrix4x4.scaling(0.5, 0.5, 0.5).translate(1.5, 0.5, -0.5),
      new Material(new Color(0.5, 1, 0.1), undefined, 0.7, 0.3),
    );

    const left = new Sphere(
      Matrix4x4.scaling(0.33, 0.33, 0.33).translate(-1.5, 0.33, -0.75),
      new Material(new Color(1, 0.8, 0.1), undefined, 0.7, 0.3),
    );

    const light = new PointLight(new Point(-10, 10, -10), Color.White);
    const world = new World(light, [floor, middle, right, left]);

    const cameraTransform = viewTransform(new Point(0, 1.5, -5), new Point(0, 1, 0), new Vector(0, 1, 0));
    const camera = new Camera(100, 50, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);

    saveCanvas(canvas, 'ch09-planes.ppm');
  });

  it('Chapter 10 - Stripes', () => {
    const pattern = new StripePattern(Color.White, new Color(1, 0.7529, 0.796));
    const stripedMaterial = new Material().withPattern(pattern);
    const floor = new Plane(undefined, stripedMaterial);
    const wall = new Plane(Matrix4x4.rotationX(Math.PI / 2).translate(0, 0, 5), stripedMaterial);

    const middle = new Sphere(
      Matrix4x4.translation(-0.5, 1, 0.5),
      new Material(
        new Color(0.1, 1, 0.5),
        undefined,
        0.7,
        0.3,
        undefined,
        undefined,
        undefined,
        undefined,
        pattern.withTransform(Matrix4x4.scaling(0.25, 0.25, 0.25)),
      ),
    );

    const right = new Sphere(
      Matrix4x4.scaling(0.5, 0.5, 0.5).translate(1.5, 0.5, -0.5),
      new Material(new Color(0.5, 1, 0.1), undefined, 0.7, 0.3, undefined, undefined, undefined, undefined, pattern),
    );

    const left = new Sphere(
      Matrix4x4.scaling(0.33, 0.33, 0.33).translate(-1.5, 0.33, -0.75),
      new Material(new Color(1, 0.8, 0.1), undefined, 0.7, 0.3, undefined, undefined, undefined, undefined, pattern),
    );

    const light = new PointLight(new Point(-10, 10, -10), Color.White);
    const world = new World(light, [floor, wall, middle, right, left]);

    const cameraTransform = viewTransform(new Point(-1.5, 1.5, -5), new Point(0, 1, 0), new Vector(0, 1, 0));
    const camera = new Camera(100, 50, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);

    saveCanvas(canvas, 'ch10-stripes.ppm');
  });

  it('Chapter 11 - Reflection', () => {
    const floor = new Plane(
      undefined,
      new Material().withPattern(new CheckerPattern(Color.White, Color.Red)).withReflective(0.5),
    );

    const leftWall = new Plane(
      Matrix4x4.rotationX(Math.PI / 2).rotateZ(Math.PI / 2),
      new Material().withPattern(new StripePattern(Color.White, Color.Black)),
    );

    const rightWall = new Plane(
      Matrix4x4.rotationZ(Math.PI / 2),
      new Material().withPattern(new StripePattern(Color.White, Color.Black)),
    );

    const middle = new Sphere(
      Matrix4x4.translation(-3, 1, -3),
      new Material(new Color(0.1, 1, 0.5), undefined, 0.7, 0.3, 20, 0.25),
    );

    const right = new Sphere(
      Matrix4x4.scaling(0.5, 0.5, 0.5).translate(-4.5, 0.5, -3.5),
      new Material(new Color(0.5, 1, 0.1), undefined, 0.7, 0.3),
    );

    const left = new Sphere(
      Matrix4x4.scaling(0.33, 0.33, 0.33).translate(-1.5, 0.33, -4.0),
      new Material(new Color(1, 0.8, 0.1), undefined, 0.7, 0.3),
    );

    const light = new PointLight(new Point(-8, 6, -4), Color.White);
    const world = new World(light, [floor, leftWall, rightWall, middle, right, left]);

    const cameraTransform = viewTransform(new Point(-6, 1, -8), new Point(0, 1, 0), new Vector(0, 1, 0));
    const camera = new Camera(100, 50, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);
    saveCanvas(canvas, 'ch11-reflection.ppm');
  });

  it('Chapter 11 - Refraction without Fresnel', () => {
    const floor = new Plane(
      Matrix4x4.translation(0, -1, 0),
      new Material().withSpecular(0).withPattern(new CheckerPattern(Color.White, Color.Black)),
    );

    const wall = new Plane(
      Matrix4x4.rotationX(Math.PI / 2).translate(0, 0, 2),
      new Material().withSpecular(0).withPattern(new CheckerPattern(Color.White, Color.Black)),
    );

    const outerSphere = createGlassSphere();
    const innerSphere = new Sphere(undefined, new Material().withRefractiveIndex(1)).withTransform(
      Matrix4x4.translation(0, 0.0625, 0).scale(0.4, 0.4, 0.4),
    );

    const light = new PointLight(new Point(0, 6, 0), Color.White);
    const world = new World(light, [floor, wall, outerSphere, innerSphere]);

    // Look directly down
    const cameraTransform = viewTransform(new Point(0, 0, -3), new Point(0, 0, 0), new Vector(0, 1, 0));
    const camera = new Camera(50, 50, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);
    saveCanvas(canvas, 'ch11-refraction-no-fresnel.ppm');
  });
});

describe('Current Chapter Test', () => {
  it('Chapter 11 - Chapter head', () => {
    const floor = new Plane(
      Matrix4x4.translation(0, -1, 0),
      new Material().withReflective(0.5).withPattern(new CheckerPattern(Color.White, Color.Black)),
    );

    const leftWall = new Plane(
      Matrix4x4.rotationX(Math.PI / 2)
        .rotateZ(Math.PI / 2)
        .translate(0, 0, 4),
      new Material()
        .withReflective(0.5)
        .withPattern(new StripePattern(Color.Gray, Color.DarkGray, Matrix4x4.scaling(0.75, 0.75, 0.75))),
    );

    const rightWall = new Plane(
      Matrix4x4.rotationZ(Math.PI / 2).translate(7, 0, 0),
      new Material()
        .withReflective(0.5)
        .withPattern(new StripePattern(Color.Gray, Color.DarkGray, Matrix4x4.scaling(0.75, 0.75, 0.75))),
    );

    const orangeBall = new Sphere(
      Matrix4x4.scaling(1.25, 1.25, 1.25),
      new Material(new Color(1.0, 0.25, 0)).withSpecular(0.3).withShininess(5),
    );

    const greenBall = new Sphere(
      Matrix4x4.scaling(0.5, 0.5, 0.5).translate(-2, 0, 2),
      new Material(new Color(0, 1, 0.75)),
    );

    const blueBall = new Sphere(
      Matrix4x4.translation(-8.5, 0, 2).scale(0.33, 0.33, 0.33),
      new Material(new Color(0, 0.75, 1)),
    );

    const light = new PointLight(new Point(-10, 6, -5), Color.White);
    const world = new World(light, [floor, leftWall, rightWall, orangeBall, greenBall, blueBall]);

    const cameraTransform = viewTransform(new Point(-3.0, 1.0, -6), new Point(0, 0, 0), new Vector(0, 1, 0));
    const camera = new Camera(200, 100, Math.PI / 3, cameraTransform);

    const canvas = render(camera, world);

    saveCanvas(canvas, 'ch11-chapter-head.ppm');
  });
});
