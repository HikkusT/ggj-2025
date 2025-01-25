// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// [CustomEditor(typeof(Ingredient))]
// public class IngredientEditor : Editor
// {
//     private VisualElement rootElement;
//
//     public override VisualElement CreateInspectorGUI()
//     {
//         rootElement = new VisualElement();
//         
//         // Add a title for the editor window
//         var titleLabel = new Label("Ingredient Effects Timeline");
//         titleLabel.style.fontSize = 20;
//         titleLabel.style.marginBottom = 10;
//         rootElement.Add(titleLabel);
//         
//         // Get the target ingredient object
//         var ingredient = (Ingredient)target;
//         
//         // Create a scrollable container for the effects timeline
//         var timelineContainer = new VisualElement();
//         timelineContainer.style.flexDirection = FlexDirection.Column;
//         timelineContainer.style.maxHeight = 400;
//         timelineContainer.style.overflow = Overflow.Visible;
//         rootElement.Add(timelineContainer);
//
//         // Add each effect as a row in the timeline
//         if (ingredient.IngredientEffects != null)
//         {
//             foreach (var effect in ingredient.IngredientEffects)
//             {
//                 var effectRow = CreateEffectRow(effect);
//                 timelineContainer.Add(effectRow);
//             }
//         }
//
//         // Add the button to add a new effect
//         var addEffectButton = new Button(() =>
//         {
//             ingredient.IngredientEffects.Add(new TimedIngredientEffect());
//             EditorUtility.SetDirty(target);  // Mark the SO as dirty
//             AssetDatabase.SaveAssets();     // Save the asset
//             Repaint();  // Repaint the inspector to show the new effect
//         })
//         {
//             text = "Add Effect"
//         };
//         rootElement.Add(addEffectButton);
//
//         return rootElement;
//     }
//
//     private VisualElement CreateEffectRow(TimedIngredientEffect effect)
//     {
//         var row = new VisualElement();
//         row.style.flexDirection = FlexDirection.Row;
//         row.style.marginBottom = 5;
//         row.style.paddingTop = 5;
//         
//         // Time input field
//         var timeField = new FloatField("Time")
//         {
//             value = effect.Time
//         };
//         timeField.RegisterValueChangedCallback(evt =>
//         {
//             // effect._time = evt.newValue;
//             EditorUtility.SetDirty(target);
//         });
//         row.Add(timeField);
//
//         // Color input field (if it affects color)
//         if (effect.Effect?.Color != null)
//         {
//             var colorField = new ColorField("Color")
//             {
//                 value = effect.Effect.Color.Value
//             };
//             colorField.RegisterValueChangedCallback(evt =>
//             {
//                 // effect.Effect._color = evt.newValue;
//                 EditorUtility.SetDirty(target);
//             });
//             row.Add(colorField);
//         }
//
//         return row;
//     }
// }
