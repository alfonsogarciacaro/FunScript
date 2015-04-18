namespace FunScript.Core

type Func<'T1> = delegate of unit->'T1
type Func<'T1,'T2> = delegate of 'T1->'T2
type Func<'T1,'T2,'T3> = delegate of 'T1*'T2->'T3
type Func<'T1,'T2,'T3,'T4> = delegate of 'T1*'T2*'T3->'T4
type Func<'T1,'T2,'T3,'T4,'T5> = delegate of 'T1*'T2*'T3*'T4->'T5
type Func<'T1,'T2,'T3,'T4,'T5,'T6> = delegate of 'T1*'T2*'T3*'T4*'T5->'T6
type Func<'T1,'T2,'T3,'T4,'T5,'T6,'T7> = delegate of 'T1*'T2*'T3*'T4*'T5*'T6->'T7
type Func<'T1,'T2,'T3,'T4,'T5,'T6,'T7,'T8> = delegate of 'T1*'T2*'T3*'T4*'T5*'T6*'T7->'T8
type Func<'T1,'T2,'T3,'T4,'T5,'T6,'T7,'T8,'T9> = delegate of 'T1*'T2*'T3*'T4*'T5*'T6*'T7*'T8->'T9
type Func<'T1,'T2,'T3,'T4,'T5,'T6,'T7,'T8,'T9,'T10> = delegate of 'T1*'T2*'T3*'T4*'T5*'T6*'T7*'T8*'T9->'T10
