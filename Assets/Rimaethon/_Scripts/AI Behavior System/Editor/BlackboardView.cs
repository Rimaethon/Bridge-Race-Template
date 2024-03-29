using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rimaethon._Scripts.AI_Behavior_System.Editor {
    public class BlackboardView : VisualElement {
        public new class UxmlFactory : UxmlFactory<BlackboardView, VisualElement.UxmlTraits> { }

        public BlackboardView() {

        }

        internal void Bind(SerializedBehaviourTree serializer) {
            Clear();

            var blackboardProperty = serializer.Blackboard;

            blackboardProperty.isExpanded = true;

            // Property field
            PropertyField field = new PropertyField();
            field.BindProperty(blackboardProperty);
            Add(field);
        }
    }
}