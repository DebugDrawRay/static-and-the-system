using UnityEngine;
using System.Collections;

public interface actionsController
{
    void onEnter();
    void update(IinputListener input);
    void onExit();
}
