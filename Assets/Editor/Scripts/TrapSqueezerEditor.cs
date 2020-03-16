using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrapSqueezer))]
public class TrapSqueezerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        TrapSqueezer squeezer = target as TrapSqueezer;
        EditorGUILayout.Space();
        GUILayout.Label("Squeezer Editor", GUI.skin.textArea);
        EditorGUILayout.Space();

        float promotion = EditorGUILayout.Slider("Promotion", CalculatePromotion(squeezer), 0.0f, 1.0f);
        Mathf.Clamp(promotion, 0.0f, 1.0f);
        CalculatePosition(squeezer, promotion);

        squeezer.Design.SetDesign(EditorGUILayout.ObjectField("Design", squeezer.Design.GetDesign(), typeof(ScriptableSqueezerDesign), true) as ScriptableSqueezerDesign);
        if (squeezer.Design.GetDesign() != null && squeezer.Design.GetDesign().GetSizeCount() > 0)
        {

            squeezer.Design.Size = EditorGUILayout.IntSlider("Size", squeezer.Design.Size, 0, squeezer.Design.GetDesign().GetSizeCount()-1);
            squeezer.Design.AppyDesign();
        }
    }
    protected void CalculatePosition(TrapSqueezer _squeezer, float _promotion)
    {
        if (_squeezer.Movement.MoveState == SqueezerMovementState.up)
        {
            UpdatePress(_squeezer, (_squeezer.Movement.StartPoint - _squeezer.Movement.EndPoint) * _promotion + _squeezer.Movement.EndPoint);
        }
        else
        {
            UpdatePress(_squeezer, (_squeezer.Movement.EndPoint - _squeezer.Movement.StartPoint) * _promotion + _squeezer.Movement.StartPoint);
        }
    }
    protected float CalculatePromotion(TrapSqueezer _squeezer)
    {
        float distance = (_squeezer.Movement.EndPoint - _squeezer.Movement.StartPoint).magnitude;
        float promDistance;
        if (_squeezer.Movement.MoveState == SqueezerMovementState.up)
        {
            promDistance = (_squeezer.Movement.EndPoint - (Vector2)_squeezer.Movement.GetPress().localPosition).magnitude;
        }
        else
        {
            promDistance = (_squeezer.Movement.StartPoint - (Vector2)_squeezer.Movement.GetPress().localPosition).magnitude;
        }
        return Mathf.Clamp(promDistance / distance, 0.0f, 1.0f);
    }
    protected void UpdatePress(TrapSqueezer _squeezer, Vector3 _pos)
    {
        _squeezer.Animation.SetSize(-_pos.y);
        _squeezer.Movement.GetPress().localPosition = _pos;
    }
    protected virtual void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            TrapSqueezer squeezer = target as TrapSqueezer;

            EditorGUI.BeginChangeCheck();
            {
                //float size = HandleUtility.GetHandleSize(squeezer.GetPosition() + squeezer.Movement.EndPoint) ;
                Vector3 snap = Vector3.one * 0.1f;
                Vector2 newTargetPosition = Handles.FreeMoveHandle(squeezer.GetPosition() + squeezer.Movement.EndPoint, Quaternion.identity, 0.1f, snap, Handles.RectangleHandleCap);
                newTargetPosition -= squeezer.GetPosition();
                newTargetPosition.x = 0;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(squeezer, "Change Squeezer End Position");
                    squeezer.Movement.EndPoint = newTargetPosition;
                    if (squeezer.Movement.StartPoint.y - 0.1f < newTargetPosition.y)
                        squeezer.Movement.EndPoint = squeezer.Movement.StartPoint + Vector2.down * 0.1f;
                    if(squeezer.Movement.GetPress().localPosition.y < newTargetPosition.y)
                        UpdatePress(squeezer,squeezer.Movement.EndPoint);
                }
            }
        }
    }
}
