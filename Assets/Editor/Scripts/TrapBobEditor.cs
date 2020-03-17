using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrapBob))]
public class TrapBobEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        TrapBob bob = target as TrapBob;
        if (bob.enabled == true && !Application.isPlaying)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Bob Editor", GUI.skin.textArea);
            EditorGUILayout.Space();

            float promotion = EditorGUILayout.Slider("Promotion", CalculatePromotion(bob), 0.0f, 1.0f);
            Mathf.Clamp(promotion, 0.0f, 1.0f);
            CalculatePosition(bob, promotion);

            bob.Design.SetDesign(EditorGUILayout.ObjectField("Design", bob.Design.GetDesign(), typeof(ScriptableBobDesign), true) as ScriptableBobDesign);
            if (bob.Design.GetDesign() != null && bob.Design.GetDesign().GetSizeCount() > 0)
            {

                bob.Design.Size = EditorGUILayout.IntSlider("Size", bob.Design.Size, 0, bob.Design.GetDesign().GetSizeCount() - 1);
                bob.Design.AppyDesign();
            }
        }
    }
    protected void CalculatePosition(TrapBob _bob, float _promotion)
    {
        //if (_squeezer.Movement.MoveState == SqueezerMovementState.up)
        //{
        //    UpdatePress(_squeezer, (_squeezer.Movement.StartPoint - _squeezer.Movement.EndPoint) * _promotion + _squeezer.Movement.EndPoint);
        //}
        //else
        //{
        //    UpdatePress(_squeezer, (_squeezer.Movement.EndPoint - _squeezer.Movement.StartPoint) * _promotion + _squeezer.Movement.StartPoint);
        //}
    }
    protected float CalculatePromotion(TrapBob _bob)
    {
        float promDistance = _bob.GetRotation() + _bob.Movement.Angle / 2.0f;
        return Mathf.Clamp(promDistance / _bob.Movement.Angle, 0.0f, 1.0f);
    }
    protected void UpdateSphere(TrapBob _bob, Vector3 _pos)
    {
        _bob.Design.SetStickLenght(-_pos.y);
        _bob.Design.GetSphere().localPosition = _pos;
    }
    protected virtual void OnSceneGUI()
    {
        TrapBob bob = target as TrapBob;
        if (bob.enabled == true && !Application.isPlaying)
        {

            EditorGUI.BeginChangeCheck();
            {
                //float size = HandleUtility.GetHandleSize(squeezer.GetPosition() + squeezer.Movement.EndPoint) ;
                Vector3 snap = Vector3.one * 0.1f;
                Vector2 newTargetPosition = Handles.FreeMoveHandle(bob.GetPosition() + (Vector2)bob.Design.GetSphere().localPosition, Quaternion.identity, 0.1f, snap, Handles.RectangleHandleCap);
                newTargetPosition -= bob.GetPosition();
                newTargetPosition.x = 0;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(bob, "Change Bob Sphere Position");
                    if (newTargetPosition.y >= -bob.Design.GetSphereRadius())
                        UpdateSphere(bob, new Vector2(0.0f, -bob.Design.GetSphereRadius()));
                    else
                    UpdateSphere(bob, newTargetPosition);
                }
            }
        }
    }
}
