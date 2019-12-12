# BattleSimulator

App simulator of the battle between 10 and n armies.

Once at least 10 armies have joined, the battle can start. When the start action is called, the armies start to attack.

# Attack and strategies:

Random: Attack a random army

Weakest: Attack the army with the lowest number of units

Strongest: Attack the army with the highest number of units


# Attack chances
Not every attack is successful. Army has 1% of success for every alive unit in it.

# Attack damage
The army always does 0.5 damage per unit, when an attack is successful. 

# Received damage
For every whole point of received damage from the attacking army, one unit is removed from the attacked army.

# Reload time
Reload time takes 0.01 seconds per 1 unit in the army.

# Rules
The army is dead (defeated) when all units are dead. 
The battle is over when there is one army standing.

# Persistence
If the app stops at any moment, or the user stops it manually (kill the process) starting it again,
the app must continue from the previous state.
The same concept is also applied to reloads and attacks.
If a reload is interrupted at any time when the application is started again, that army reload will continue from the same moment.
