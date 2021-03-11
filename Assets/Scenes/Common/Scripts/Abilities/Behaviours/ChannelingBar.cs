using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelingBar : BaseBehaviour
{
    private Slider _slider = null;
    private Image _fill = null;

    private Color _fillcolor;
    private Coroutine fade;
    
    public override BaseBehaviourData Data { get => data; set => data = (ChannelingBarData)value; }
    [SerializeField] private ChannelingBarData data;

	public override void Initialize()
	{
        Transform reference = transform.parent.transform.Find("WorldSpaceUI").transform.Find("Canvas").transform.Find("ChannelingBar");

        _slider = reference.GetComponent<Slider>();
        _fill = reference.Find("Fill").GetComponent<Image>();
        _fillcolor = _fill.color;
	}


	public override void OnEnter(BaseAbilityState.AbilityStateContext ctx)
	{
        if (fade != null)
        {
            _slider.StopCoroutine(fade);
            _fill.color = _fillcolor;
            fade = null;
        }

        UpdateSlider(ctx.duration, ctx.elapsedTime);
	}

	public override void OnExit(BaseAbilityState.AbilityStateContext ctx)
	{
        fade = _slider.StartCoroutine(SliderCancelFade(ctx));
	}

    private IEnumerator SliderCancelFade(BaseAbilityState.AbilityStateContext ctx)
    {
        if (!ctx.stateCompleted) _fill.color = new Color(1f, 0f, 0f);

        float elapsedTime = 0f;
        float duration = .5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _fill.color = new Color(_fill.color.r, _fill.color.g, _fill.color.b, 1f - (elapsedTime/duration));
            yield return 0;
        }

        _fill.color = _fillcolor;
        UpdateSlider(ctx.duration, 0f);
        fade = null;
    }

	public override void Tick(BaseAbilityState.AbilityStateContext ctx)
	{
        UpdateSlider(ctx.duration, ctx.elapsedTime);
	}

    private void UpdateSlider(float duration, float elapsedTime)
    {
        _slider.value = elapsedTime / duration;
    }
}
