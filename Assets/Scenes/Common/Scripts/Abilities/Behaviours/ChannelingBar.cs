using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChannelingBar : BaseBehaviour
{
    private Slider _slider = null;
    private Image _fill = null;

    private float elapsedDuration;

	public ChannelingBar(ChannelingBarData data) : base(data) {}

	public override object Clone()
	{
		return new ChannelingBar((ChannelingBarData)Data);
	}

	public override void OnEnter()
	{
        if (_slider == null || _fill == null)
        {
            // TODO: this should be called in constructor, but it throws errors because statemachine has no owner yet
            Transform reference = stateMachine.owner.transform.Find("WorldSpaceUI").transform.Find("Canvas").transform.Find("ChannelingBar");

            _slider = reference.GetComponent<Slider>();
            _fill = reference.Find("Fill").GetComponent<Image>();
        }

        elapsedDuration = 0f;
        UpdateSlider();
	}

	public override void OnExit()
	{
        elapsedDuration = 0f;
        UpdateSlider();
	}

	public override void Tick()
	{
        elapsedDuration += Time.deltaTime;
        UpdateSlider();
	}

    private void UpdateSlider()
    {
        _slider.value = elapsedDuration / stateMachine.channelDuration;
    }
}
