# Survivors — прототип по референсу Vampire Survivors

Unity 6 (6000.6.4f1), 2D, встроенный рендер-пайплайн, старый Input Manager.

## Как запустить
1. Unity Hub → **Add** → папка `Documents/SurvivorsGame`.
2. Открыть сцену `Assets/Game/Scenes/Game.unity` → **Play**.
3. Управление: **WASD / стрелки**. Оружие стреляет само. После смерти — кнопка **Restart** или **R**.

Если сцену нужно пересобрать с нуля: меню **Tools → Survivors → Build Game**
(перезапишет сцену, спрайты и префабы).

## Где что сделано

| Пункт | Что | Где |
|---|---|---|
| 1. Player | SpriteRenderer + BoxCollider2D + Rigidbody2D, движение X/Y, `moveSpeed` в Inspector, `freezeRotation` | `PlayerController.cs`, объект `Player` |
| 2. Enemy | Префабы с SpriteRenderer + Collider2D, идут к игроку, `moveSpeed` в Inspector | `Enemy.cs`, `Prefabs/Enemy_*.prefab` |
| 3. Health и Damage | HP у игрока и врагов, враг исчезает при HP ≤ 0, урон при контакте (раз в `damageInterval`), смерть игрока → Game Over | `Health.cs`, `Enemy.cs` |
| 4. Automatic Weapon | Projectile-префаб, выстрел каждые `attackInterval` в ближайшего врага, исчезает после попадания или `lifetime`; `damage`, `projectileSpeed`, `attackInterval` в Inspector | `AutoWeapon.cs`, `Projectile.cs` |
| 5. Enemy Spawner | Спавн вокруг игрока, `Spawn Interval`, `min/max Spawn Distance` | `EnemySpawner.cs` |
| 6. Gameplay Loop | Score + Kills, Game Over, Restart, враги появляются сразу при старте (`initialEnemies`) | `GameManager.cs` |
| **Ветка A** | | |
| 7A. HUD | HP Bar, Score, Timer | `GameManager.cs`, `UIBar.cs`, Canvas |
| 8A. XP | XP pickup выпадает из врага, сбор, XP Bar, Level | `XPPickup.cs`, `XPDropper.cs`, `PlayerLevel.cs` |
| 9A. Level Up | Пауза (`Time.timeScale = 0`) + 3 кнопки: +Damage, +Attack Speed, +Movement Speed | `GameManager.cs`, `LevelUpPanel` |
| **Ветка B** | | |
| 7B. Spawn Distribution | Случайная точка на кольце между min и max дистанцией | `EnemySpawner.Spawn()` |
| 8B. Enemy Types | `Enemy_Normal`, `Enemy_Fast`, `Enemy_Tank`, выбор по весам | `EnemySpawner.PickType()` |
| 9B. Difficulty | Со временем: интервал спавна ↓, вес Fast/Tank ↑, лимит активных врагов ↑ (`rampDuration`) | `EnemySpawner.Update()` |
| 10. Hit Feedback | Вспышка цветом + scale punch + knockback | `Health.cs`, `Enemy.OnDamaged()` |
| 11. Death Polish | Particle System (цвет врага), анимация смерти (сжатие + вращение), звук | `EnemyDeathEffects.cs`, `Enemy.DeathAnimation()`, `SoundFX.cs` |
| 12. Advanced System | **Object Pool** для projectiles + **event-based Enemy Death** (`Enemy.AnyEnemyDied`: на него подписаны счёт, выпадение XP и эффекты) | `ProjectilePool.cs`, `Enemy.cs` |

Сделаны обе ветки (A и B) — какая бы ни досталась, она уже есть.

## Типы врагов (8B)

| Тип | HP | Скорость | Урон | Очки | XP | Вес в начале → в конце |
|---|---|---|---|---|---|---|
| Normal (красный) | 3 | 2.0 | 1 | 1 | 1 | 1 → 1 |
| Fast (жёлтый, маленький) | 1 | 3.6 | 1 | 2 | 1 | 0.15 → 0.9 |
| Tank (фиолетовый, большой) | 12 | 1.1 | 2 | 5 | 3 | 0.05 → 0.6 |

Все значения можно менять в Inspector на префабах и на объекте `EnemySpawner`.

## Звуки
Звуки генерируются кодом (`SoundFX.cs`), чтобы не нужны были файлы. Свои `.wav` можно перетащить
в поля компонента `SoundFX` на `Main Camera` — тогда будут использоваться они.
