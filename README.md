# PinionCore.Utility
[![Maintainability](https://api.codeclimate.com/v1/badges/aecf8a766e61566e9870/maintainability)](https://codeclimate.com/github/jiowchern/PinionCore.Utility/maintainability)

Common utility library for the [PinionCore](https://github.com/jiowchern/PinionCore.Remote) ecosystem — foundational types for state management, reactive object collections, memory pooling, and console tooling shared across PinionCore modules.

## Modules

| Namespace / Folder | Contents |
|---|---|
| `Remote/` | Reactive object supply: `INotifier<out T>` / `Depot<T>` (supply/unsupply object collections), `Spirit<T>` (single-shot disposable object supply returned from methods), `IAwaitable`, ghost/provider plumbing shared with PinionCore.Remote |
| `Utility/` | `StageMachine` / `StatusMachine` (state machines), `Command` family (console command registration and parsing), `Console` / `ConsoleViewer` / `ConsoleInput`, `Log` / `LogFileRecorder`, `Launcher` / `Bootable`, `Updater`, `PowerRegulator`, `BilateralMap`, misc. helpers |
| `Memorys/` | Pooled buffers: `Pool` / `ChunkPool` / `PooledBuffer` for allocation-free byte handling |
| `Serialization/` | `Varint` and `ZigZag` integer encodings |
| `Collection/` | `DifferenceNoticer` (diff-based add/remove notification), thread-safe `Queue` |
| `Extensions/` | LINQ, type, and number extension helpers |

## Notifier covariance

`INotifier<T>` is covariant (`INotifier<out T>`): a notifier of a concrete type implicitly converts to a notifier of any interface that type implements — e.g. `INotifier<World>` → `INotifier<IWorld>` — with the constraint checked at compile time.

To support this, `Depot<T>` and `TProvider<T>` store event handlers in lists rather than multicast delegate fields: subscribing through a covariant reference passes a delegate whose runtime type differs from the backing field's, which makes `Delegate.Combine` throw `ArgumentException`. Keep this in mind when writing your own `INotifier<T>` implementation — field-like events have the same trap when subscribed through a variant reference.

## Usage

This library targets `netstandard2.1` and is consumed as a submodule / package by [PinionCore.Remote](https://github.com/jiowchern/PinionCore.Remote); see that repository's documentation for end-to-end usage.
