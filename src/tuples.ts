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

const Epsilon = 0.00001;

function floatEqual(a: number, b: number): boolean {
  return Math.abs(a - b) < Epsilon;
}

export function areTuplesEqual(a: Tuple, b: Tuple): boolean {
  return floatEqual(a.x, b.x) && floatEqual(a.y, b.y) && floatEqual(a.z, b.z) && floatEqual(a.w, b.w);
}

export function tupleAdd(a: Tuple, b: Tuple): Tuple {
  return { x: a.x + b.x, y: a.y + b.y, z: a.z + b.z, w: a.w + b.w };
}

export function tupleSubtract(a: Tuple, b: Tuple): Tuple {
  return { x: a.x - b.x, y: a.y - b.y, z: a.z - b.z, w: a.w - b.w };
}

export function tupleNegate(a: Tuple): Tuple {
  return { x: -a.x, y: -a.y, z: -a.z, w: -a.w };
}

export function tupleMultiply(a: Tuple, scalar: number): Tuple {
  return { x: a.x * scalar, y: a.y * scalar, z: a.z * scalar, w: a.w * scalar };
}

export function tupleDivide(a: Tuple, scalar: number): Tuple {
  return { x: a.x / scalar, y: a.y / scalar, z: a.z / scalar, w: a.w / scalar };
}
