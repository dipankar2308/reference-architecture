# ADR 001: Choose Clean Architecture + Hexagonal Pattern

## Context
Building enterprise applications that need to evolve independently of infrastructure and database choices.

## Decision
Adopt Clean Architecture with Hexagonal (Ports & Adapters) pattern.

## Status
Accepted

## Consequences
[+] Business logic isolated from frameworks
[+] Easy to test
[+] Infrastructure can be swapped
[-] More boilerplate initially
