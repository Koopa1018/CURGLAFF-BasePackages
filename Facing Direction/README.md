# Facing Direction
Simple data components for Unity which store and represent a direction an object is facing.

Variants are available both for systems which do and for systems which do not need to know when objects are facing upwards.
Additionally, an integer-based variant is provided for systems that only care *whether* an object is facing up/down and left/right, not *how much* of each axis is being used.

# Checkout URLs
To add a Facing Direction package, first add the Facing Direction Base package:
```
https://github.com/Koopa1018/CURGLAFF-BasePackages.git?path=/Facing Direction/Facing Direction Base#5129c2ad945b161137076d739054c454e4e966eb
```

Then add the URL of the package you need (details on the packages can be found below).

*2D:*
```
https://github.com/Koopa1018/CURGLAFF-BasePackages.git?path=/Facing Direction/Facing Direction 2D5129c2ad945b161137076d739054c454e4e966eb
```

*3D:*
```
https://github.com/Koopa1018/CURGLAFF-BasePackages.git?path=/Facing Direction/Facing Direction 3D#5129c2ad945b161137076d739054c454e4e966eb
```

*8-Way:*
```
https://github.com/Koopa1018/CURGLAFF-BasePackages.git?path=/Facing Direction/Facing Direction 8 Way#5129c2ad945b161137076d739054c454e4e966eb
```

# Packages
Because not all games require the same information about which way someone or something is facing, several different Facing Direction packages are provided. A brief description of each is given here:

## Facing 2D
Facing 2D represents facing direction as a `float2` where each axis is stored individually. This is mostly useful for directly storing a character or object's forward vector.
This component is helpful in systems where characters and objects move on a plane, but don't have behaviors relying on whether or not they're looking up or down, e.g. a 2D RPG such as Chrono Trigger.

## Facing 3D
Facing 3D represents facing direction as a `float2` and a `float`: `float2` for the XY plane, `float` representing how far up/down it's looking.
This component is useful in 3D games where looking up/down changes movement (e.g. flight sims) or aim direction (e.g. shooters of any kind).

## Facing 8-Way
Facing 8-Way represents facing direction as an `int2`. Like Facing 2D, each axis is stored individually; however, the values can only consist of the eight cardinal directions, as well as `(0,0)`.
To allow analog and touch input, this component should only be used in systems where left/right facing and up/down facing interact with gameplay in completely separate ways, e.g. a 2D platformer where you can walk left and right, but only look up and down. If the game features a climbing system with unconstrained eight-directional movement, consider switching to Facing 2D while climbing is active.
