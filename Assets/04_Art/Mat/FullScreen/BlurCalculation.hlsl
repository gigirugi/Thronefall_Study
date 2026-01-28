#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void AddAllThenDivide_float(float4 In1, float4 In2, float4 In3, float4 In4, out float4 Out)
{
    Out = (In1 + In2 + In3 + In4) * 0.25;
}
#endif