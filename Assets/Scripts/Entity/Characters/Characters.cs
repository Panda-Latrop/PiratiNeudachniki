using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseCharacter : IDamageableHandler
{ }
public interface IMoveCharacter : IBaseCharacter,IMovementHandler
{ }
public interface IShootCharacter : IBaseCharacter, IShooterHandler
{ }
public interface IMoveShootCharacter : IMoveCharacter, IShootCharacter
{ }
public interface IPirateCharacter : IMoveShootCharacter, IRotationHandler
{ }
