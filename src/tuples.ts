export interface Tuple {
  readonly x: number;
  readonly y: number;
  readonly z: number;
  readonly w: number;
}

export function tuple(x: number, y: number, z: number, w: number): Tuple {
  return { x, y, z, w };
}

export function point(x: number, y: number, z: number): Tuple {
  return { x, y, z, w: 1 };
}

export function vector(x: number, y: number, z: number): Tuple {
  return { x, y, z, w: 0 };
}

export function isPoint(tuple: Tuple): boolean {
  return tuple.w === 1;
}

export function isVector(tuple: Tuple): boolean {
  return tuple.w === 0;
}
