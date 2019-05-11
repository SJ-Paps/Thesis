using UnityEngine;

public struct AnimatorParameterId
{
    private int parameterId;

    public AnimatorParameterId(string parameterName)
    {
        parameterId = Animator.StringToHash(parameterName);
    }

    public static implicit operator int(AnimatorParameterId parameterIdObj)
    {
        return parameterIdObj.parameterId;
    }
}
