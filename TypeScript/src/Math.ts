export const EPSILON = 0.00001;

export function floatEqual(a: number, b: number): boolean {
  return Math.abs(a - b) < EPSILON;
}

export function clamp(x: number, min: number, max: number): number {
  if (min > max) {
    throw new Error(`min must be less than or equal to max: min=${min}, max=${max}`);
  }

  return Math.min(max, Math.max(x, min));
}

const piOver180 = Math.PI / 180.0;

export function degressToRadians(degrees: number): number {
  return degrees * piOver180;
}
