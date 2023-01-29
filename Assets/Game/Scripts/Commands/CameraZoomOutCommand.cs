
using UnityEngine;
using DG.Tweening;

namespace Game.Commands 
{
    public class CameraZoomOutCommand : ICommand
    {
        private Transform _camera;
        private Vector3 _previousPosition;
        public CameraZoomOutCommand (Transform camera)
        {
            this._camera = camera;
        }
        public void Execute()
        {
            _camera.DOComplete();
            _previousPosition = _camera.localPosition;
            _camera.DOLocalMove(_previousPosition + new Vector3(0, 0.15f, -0.3f), 0.25f);
        }

        public void Undo()
        {
            _camera.DOComplete();
            _camera.DOLocalMove(_previousPosition, 0.25f);
        }
    }

}
