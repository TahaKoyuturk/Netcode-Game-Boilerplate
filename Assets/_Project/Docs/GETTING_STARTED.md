# Getting Started

## Purpose

Onboard a developer to a running boilerplate in minutes.

## Responsibilities

Guide package setup, project initialization, and first play session.

## Dependencies

Unity 6, internet for UGS package restore.

## Usage Examples

1. Clone repository.
2. Open in Unity 6 — allow package resolution.
3. **Studio > Setup > Initialize Project** — creates configs, scenes, build settings.
4. Open `Assets/_Project/Scenes/Bootstrap.unity`.
5. Press Play — MainMenu loads additively.

### Multiplayer editor testing (ParrelSync)

After packages resolve, use **ParrelSync > Clones Manager** to create editor clones for host/client testing alongside UGS Relay.

## Common Workflows

### First Multiplayer Test

1. Link Unity project to UGS in Dashboard (Authentication + Lobby + Relay).
2. Play from Bootstrap.
3. Click **Host** on Main Menu.
4. Second client joins via lobby ID (extend UI as needed).

## Extension Guide

Replace `MainMenuPanel` button handlers for your game flow.

## Best Practices

- Always start from Bootstrap scene (build index 0).
- Run **Studio > Validate > Run Full Validation** before milestones.
