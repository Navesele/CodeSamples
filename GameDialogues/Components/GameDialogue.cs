using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using System.Linq;
using Marshtown.Extensions;
using UnityEngine.Events;
using Marshtown.Logger;

namespace Marshtown.DialogueComponents
{
    public class GameDialogue : MonoBehaviour
    {
        public DialogueLanguage DialogueLanguage = DialogueLanguage.Russian;

        [Header("Debug"), SerializeField]
        private bool _isSkippable = true;
        [SerializeField]
        private bool _skipDialogue = false;
        [Header("Initial setup")]
        [SerializeField]
        private string _dialogueFileName;
        [SerializeField]
        private int _startLineIndex = 0;
        [SerializeField]
        private List<DialogueActorPreset> _actors;
        [Space]
        [Header("Events")]
        [SerializeField]
        private UnityEvent OnDialogueStart;
        [SerializeField]
        private UnityEvent OnGetNextDialogueLine;
        [SerializeField]
        private UnityEvent<int> OnGetDialogueLine;
        [SerializeField]
        private UnityEvent OnDialogueEnd;

        private int _lineIndex = 0;
        private Dictionary<DialogueLanguage, List<DialogueLine>> _dialogueDict = new Dictionary<DialogueLanguage, List<DialogueLine>>();
        private int _totalNumberOfLines = 0;

        private const string dialogueDirectoryName = "Dialogues";

        void Awake()
        {
            _lineIndex = _startLineIndex;
            ReadDialogueFromFile(dialogueDirectoryName, _dialogueFileName);
            _totalNumberOfLines = GetTotalNumberOfDialogueLines(_dialogueDict);

            ValidateDependencies();
        }

        public DialogueLine GetNextLine()
        {
            var line = GetLineByIndex(_lineIndex, _dialogueDict, DialogueLanguage);

            if (line != null)
            {
                OnGetNextDialogueLine.Invoke();
                OnGetDialogueLine.Invoke(_lineIndex);
                _lineIndex++;
            }

            return line;
        }

        public DialogueActorPreset GetActorPresetByInternalName(string internalName)
        {
            if (string.IsNullOrWhiteSpace(internalName))
            {
                return null;
            }

            if (_actors == null || _actors.Count == 0)
            {
                return null;
            }

            return _actors.Where(a => a.InternalName.Equals(internalName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public int GetCurrentLineIndex()
        {
            return _lineIndex;
        }

        public DialogueLine RefreshCurrentLine()
        {
            return GetLineByIndex(_lineIndex, _dialogueDict, DialogueLanguage);
        }

        public bool IsEndOfDialogue()
        {
            return _lineIndex >= _totalNumberOfLines;
        }

        public void ResetDialogue()
        {
            _lineIndex = 0;
        }

        public void StartDialogue()
        {
            OnDialogueStart.Invoke();

            if (_skipDialogue && _isSkippable)
            {
                DebugLogger.LogWarning($"Skipping dialogue: {name}", this);
                _lineIndex = _totalNumberOfLines;
            }
        }

        public void EndDialogue()
        {
            OnDialogueEnd.Invoke();
        }

        public int GetNumberOfLines()
        {
            return _totalNumberOfLines;
        }

        public void SkipDialogue()
        {
            if (!_isSkippable)
            {
                DebugLogger.LogWarning($"Dialogue {name} cannot be skipped!", this);
                return;
            }

            _skipDialogue = true;
        }

        private List<DialogueLine> ParseDialogueFile(TextAsset dialogueFile)
        {
            var resultList = new List<DialogueLine>();

            if (dialogueFile == null)
            {
                return resultList;
            }

            var story = new Story(dialogueFile.text);

            while (story.canContinue)
            {
                DialogueLine newLine = new DialogueLine(story.Continue());
                newLine.ReadTags(story.currentTags);
                resultList.Add(newLine);
            }

            return resultList;
        }

        private DialogueLine GetLineByIndex(int lineIndex, Dictionary<DialogueLanguage, List<DialogueLine>> dialogueLines, DialogueLanguage language)
        {
            if (lineIndex < 0)
            {
                return null;
            }

            if (dialogueLines == null || dialogueLines.Count == 0)
            {
                return null;
            }

            if (dialogueLines.TryGetValue(language, out List<DialogueLine> lines))
            {
                if (lines == null || lines.Count == 0)
                {
                    return null;
                }

                if (lineIndex < lines.Count)
                {
                    return lines[lineIndex];
                }

                return null;
            }

            return null;
        }

        private void ReadDialogueFromFile(string directoryName, string dialogueFileName)
        {
            _dialogueDict.Clear();

            if (string.IsNullOrWhiteSpace(dialogueFileName))
            {
                return;
            }

            var values = Enum.GetValues(typeof(DialogueLanguage));

            foreach (DialogueLanguage language in values)
            {
                string filePath = $"{directoryName}/{language.ToStringValue()}/{dialogueFileName}";
                var jsonFile = Resources.Load<TextAsset>(filePath);

                if (jsonFile != null)
                {
                    _dialogueDict.Add(language, ParseDialogueFile(jsonFile));
                }
            }
        }

        private int GetTotalNumberOfDialogueLines(Dictionary<DialogueLanguage, List<DialogueLine>> dialogueDict)
        {
            if (dialogueDict == null || dialogueDict.Count == 0)
            {
                return 0;
            }
            else
            {
                return dialogueDict.First().Value.Count;
            }
        }

        #region Dependency validation
        [System.Diagnostics.Conditional("CUSTOM_DEPENDENCY_VALIDATION")]
        private void ValidateDependencies()
        {
            if (_dialogueDict.Count == 0)
            {
                DebugLogger.LogValidationError("Dialogue dictionary is empty", this);
            }

            var numberOfLines = new Dictionary<DialogueLanguage, int>();

            foreach (var line in _dialogueDict)
            {
                numberOfLines.Add(line.Key, line.Value.Count);
            }

            if (numberOfLines.Values.Any(v => v != _totalNumberOfLines))
            {
                DebugLogger.LogValidationError($"Dialogue {_dialogueFileName} has different number of lines:", this);

                foreach (var line in numberOfLines)
                {
                    DebugLogger.LogValidationError($"--> {line.Key}: {line.Value}", this);
                }
            }
        }
        #endregion Dependency validation
    }
}
