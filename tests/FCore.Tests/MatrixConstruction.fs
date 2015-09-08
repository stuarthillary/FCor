namespace FCore.Tests

open FCore
open Xunit
open FsCheck
open FsCheck.Xunit
open System
open Util

module MatrixConstruction =
    let inline (<=>) (x : float[,]) (y :float[,]) = epsEqualArray2D x y epsEqualFloat 0.0

    [<Property>]
    let ``Constructs empty matrix`` () =
        let v = Matrix.Empty
        v.LongRowCount = 0L && v.LongColCount = 0L && v.ToArray2D() = Array2D.zeroCreate<float> 0 0

    [<Property>]
    let ``Throws arg exception if construction with row64 or col64 < 0`` (r : int64) (c : int64) (x:float) =
        (r < 0L || c < 0L) ==> Prop.throws<ArgumentException, _> (lazy(new Matrix(r, c, x)))

    [<Property>]
    let ``Throws arg exception if construction with row or col < 0`` (r : int) (c: int) (x : float) =
        (r < 0 || c < 0) ==> Prop.throws<ArgumentException, _> (lazy(new Matrix(r, c, x)))

    [<Property>]
    let ``Constructs Matrix from non negative int64s and bool value`` (r : int64) (c : int64) (x:float) =
        (r >= 0L && c >= 0L) ==> lazy(let v = new Matrix(r, c, x) in v.LongRowCount = r && v.LongColCount = c && v.ToArray2D() <=> Array2D.create (int(r)) (int(c)) x)

    [<Property>]
    let ``Constructs Matrix from non negative ints and bool value`` (r : int) (c : int) (x:float) =
        (r >= 0 && c >= 0) ==> lazy(let v = new Matrix(r, c, x) in v.RowCount = r && v.ColCount = c && v.ToArray2D() <=> Array2D.create r c x)

    [<Property>]
    let ``Constructs Matrix from bool value`` (x : float) =
        let v = new Matrix(x) in v.RowCount = 1 && v.ColCount = 1 && v.ToArray2D() <=> ([[x]]|>array2D)

    [<Property>]
    let ``Constructs Matrix from bool array2d`` (data : float[,]) = 
        let v = new Matrix(data) in v.RowCount = (data|>Array2D.length1) && v.ColCount = (data|>Array2D.length2) && v.ToArray2D() <=> data

    [<Property>]
    let ``Constructs Matrix from ints and initializer function`` (r : int) (c : int) (init : int -> int -> float) = 
        (r >= 0 && c >= 0) ==> lazy (let v = new Matrix(r, c, init) in v.RowCount = r && v.ColCount = c && v.ToArray2D() <=> Array2D.init r c init)
