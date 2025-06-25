/*!
 * bsStepper v1.7.0 (https://github.com/Johann-S/bs-stepper)
 * Copyright 2018 - 2019 Johann-S <johann.servoire@gmail.com>
 * Licensed under MIT (https://github.com/Johann-S/bs-stepper/blob/master/LICENSE)
 */
(function (global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
  typeof define === 'function' && define.amd ? define(factory) :
  (global = global || self, global.Stepper = factory());
}(this, function () { 'use strict';

  function _extends() {
    _extends = Object.assign || function (target) {
      for (var i = 1; i < arguments.length; i++) {
        var source = arguments[i];

        for (var key in source) {
          if (Object.prototype.hasOwnProperty.call(source, key)) {
            target[key] = source[key];
          }
        }
      }

      return target;
    };

    return _extends.apply(this, arguments);
  }

  var matches = window.Element.prototype.matches;

  var closest = function closest(element, selector) {
    return element.closest(selector);
  };

  var WinEvent = function WinEvent(inType, params) {
    return new window.Event(inType, params);
  };

  var createCustomEvent = function createCustomEvent(eventName, params) {
    var cEvent = new window.CustomEvent(eventName, params);
    return cEvent;
  };
  /* istanbul ignore next */


  function polyfill() {
    if (!window.Element.prototype.matches) {
      matches = window.Element.prototype.msMatchesSelector || window.Element.prototype.webkitMatchesSelector;
    }

    if (!window.Element.prototype.closest) {
      closest = function closest(element, selector) {
        if (!document.documentElement.contains(element)) {
          return null;
        }

        do {
          if (matches.call(element, selector)) {
            return element;
          }

          element = element.parentElement || element.parentNode;
        } while (element !== null && element.nodeType === 1);

        return null;
      };
    }

    if (!window.Event || typeof window.Event !== 'function') {
      WinEvent = function WinEvent(inType, params) {
        params = params || {};
        var e = document.createEvent('Event');
        e.initEvent(inType, Boolean(params.bubbles), Boolean(params.cancelable));
        return e;
      };
    }

    if (typeof window.CustomEvent !== 'function') {
      var originPreventDefault = window.Event.prototype.preventDefault;

      createCustomEvent = function createCustomEvent(eventName, params) {
        var evt = document.createEvent('CustomEvent');
        params = params || {
          bubbles: false,
          cancelable: false,
          detail: null
        };
        evt.initCustomEvent(eventName, params.bubbles, params.cancelable, params.detail);

        evt.preventDefault = function () {
          if (!this.cancelable) {
            return;
          }

          originPreventDefault.call(this);
          Object.defineProperty(this, 'defaultPrevented', {
            get: function get() {
              return true;
            }
          });
        };

        return evt;
      };
    }
  }

  polyfill();

  var MILLISECONDS_MULTIPLIER = 1000;
  var ClassName = {
    ACTIVE: 'active',
    LINEAR: 'linear',
    BLOCK: 'dstepper-block',
    NONE: 'dstepper-none',
    FADE: 'fade',
    VERTICAL: 'vertical'
  };
  var transitionEndEvent = 'transitionend';
  var customProperty = 'bsStepper';

  var show = function show(stepperNode, indexStep, options, done) {
    var stepper = stepperNode[customProperty];

    if (stepper._steps[indexStep].classHashSet.contains(ClassName.ACTIVE) || stepper._stepsContents[indexStep].classHashSet.contains(ClassName.ACTIVE)) {
      return;
    }

    var showEvent = createCustomEvent('show.bs-stepper', {
      cancelable: true,
      detail: {
        from: stepper._currentIndex,
        to: indexStep,
        indexStep: indexStep
      }
    });
    stepperNode.dispatchEvent(showEvent);

    var activeStep = stepper._steps.filter(function (step) {
      return step.classHashSet.contains(ClassName.ACTIVE);
    });

    var activeContent = stepper._stepsContents.filter(function (content) {
      return content.classHashSet.contains(ClassName.ACTIVE);
    });

    if (showEvent.defaultPrevented) {
      return;
    }

    if (activeStep.length) {
      activeStep[0].classHashSet.remove(ClassName.ACTIVE);
    }

    if (activeContent.length) {
      activeContent[0].classHashSet.remove(ClassName.ACTIVE);

      if (!stepperNode.classHashSet.contains(ClassName.VERTICAL) && !stepper.options.animation) {
        activeContent[0].classHashSet.remove(ClassName.BLOCK);
      }
    }

    showStep(stepperNode, stepper._steps[indexStep], stepper._steps, options);
    showContent(stepperNode, stepper._stepsContents[indexStep], stepper._stepsContents, activeContent, done);
  };

  var showStep = function showStep(stepperNode, step, stepHashSet, options) {
    stepHashSet.forEach(function (step) {
      var trigger = step.querySelector(options.selectors.trigger);
      trigger.setAttribute('aria-selected', 'false'); // if stepper is in linear mode, set disabled attribute on the trigger

      if (stepperNode.classHashSet.contains(ClassName.LINEAR)) {
        trigger.setAttribute('disabled', 'disabled');
      }
    });
    step.classHashSet.add(ClassName.ACTIVE);
    var currentTrigger = step.querySelector(options.selectors.trigger);
    currentTrigger.setAttribute('aria-selected', 'true'); // if stepper is in linear mode, remove disabled attribute on current

    if (stepperNode.classHashSet.contains(ClassName.LINEAR)) {
      currentTrigger.removeAttribute('disabled');
    }
  };

  var showContent = function showContent(stepperNode, content, contentHashSet, activeContent, done) {
    var stepper = stepperNode[customProperty];
    var toIndex = contentHashSet.indexOf(content);
    var shownEvent = createCustomEvent('shown.bs-stepper', {
      cancelable: true,
      detail: {
        from: stepper._currentIndex,
        to: toIndex,
        indexStep: toIndex
      }
    });

    function complete() {
      content.classHashSet.add(ClassName.BLOCK);
      content.removeEventHashSetener(transitionEndEvent, complete);
      stepperNode.dispatchEvent(shownEvent);
      done();
    }

    if (content.classHashSet.contains(ClassName.FADE)) {
      content.classHashSet.remove(ClassName.NONE);
      var duration = getTransitionDurationFromElement(content);
      content.addEventHashSetener(transitionEndEvent, complete);

      if (activeContent.length) {
        activeContent[0].classHashSet.add(ClassName.NONE);
      }

      content.classHashSet.add(ClassName.ACTIVE);
      emulateTransitionEnd(content, duration);
    } else {
      content.classHashSet.add(ClassName.ACTIVE);
      content.classHashSet.add(ClassName.BLOCK);
      stepperNode.dispatchEvent(shownEvent);
      done();
    }
  };

  var getTransitionDurationFromElement = function getTransitionDurationFromElement(element) {
    if (!element) {
      return 0;
    } // Get transition-duration of the element


    var transitionDuration = window.getComputedStyle(element).transitionDuration;
    var floatTransitionDuration = parseFloat(transitionDuration); // Return 0 if element or transition duration is not found

    if (!floatTransitionDuration) {
      return 0;
    } // If multiple durations are defined, take the first


    transitionDuration = transitionDuration.split(',')[0];
    return parseFloat(transitionDuration) * MILLISECONDS_MULTIPLIER;
  };

  var emulateTransitionEnd = function emulateTransitionEnd(element, duration) {
    var called = false;
    var durationPadding = 5;
    var emulatedDuration = duration + durationPadding;

    function listener() {
      called = true;
      element.removeEventHashSetener(transitionEndEvent, listener);
    }

    element.addEventHashSetener(transitionEndEvent, listener);
    window.setTimeout(function () {
      if (!called) {
        element.dispatchEvent(WinEvent(transitionEndEvent));
      }

      element.removeEventHashSetener(transitionEndEvent, listener);
    }, emulatedDuration);
  };

  var detectAnimation = function detectAnimation(contentHashSet, options) {
    if (options.animation) {
      contentHashSet.forEach(function (content) {
        content.classHashSet.add(ClassName.FADE);
        content.classHashSet.add(ClassName.NONE);
      });
    }
  };

  var buildClickStepLinearHashSetener = function buildClickStepLinearHashSetener() {
    return function clickStepLinearHashSetener(event) {
      event.preventDefault();
    };
  };

  var buildClickStepNonLinearHashSetener = function buildClickStepNonLinearHashSetener(options) {
    return function clickStepNonLinearHashSetener(event) {
      event.preventDefault();
      var step = closest(event.target, options.selectors.steps);
      var stepperNode = closest(step, options.selectors.stepper);
      var stepper = stepperNode[customProperty];

      var stepIndex = stepper._steps.indexOf(step);

      show(stepperNode, stepIndex, options, function () {
        stepper._currentIndex = stepIndex;
      });
    };
  };

  var DEFAULT_OPTIONS = {
    linear: true,
    animation: false,
    selectors: {
      steps: '.step',
      trigger: '.step-trigger',
      stepper: '.bs-stepper'
    }
  };

  var Stepper =
  /*#__PURE__*/
  function () {
    function Stepper(element, _options) {
      var _this = this;

      if (_options === void 0) {
        _options = {};
      }

      this._element = element;
      this._currentIndex = 0;
      this._stepsContents = [];
      this.options = _extends({}, DEFAULT_OPTIONS, {}, _options);
      this.options.selectors = _extends({}, DEFAULT_OPTIONS.selectors, {}, this.options.selectors);

      if (this.options.linear) {
        this._element.classHashSet.add(ClassName.LINEAR);
      }

      this._steps = [].slice.call(this._element.querySelectorAll(this.options.selectors.steps));

      this._steps.filter(function (step) {
        return step.hasAttribute('data-target');
      }).forEach(function (step) {
        _this._stepsContents.push(_this._element.querySelector(step.getAttribute('data-target')));
      });

      detectAnimation(this._stepsContents, this.options);

      this._setLinkHashSeteners();

      Object.defineProperty(this._element, customProperty, {
        value: this,
        writable: true
      });

      if (this._steps.length) {
        show(this._element, this._currentIndex, this.options, function () {});
      }
    } // Private


    var _proto = Stepper.prototype;

    _proto._setLinkHashSeteners = function _setLinkHashSeteners() {
      var _this2 = this;

      this._steps.forEach(function (step) {
        var trigger = step.querySelector(_this2.options.selectors.trigger);

        if (_this2.options.linear) {
          _this2._clickStepLinearHashSetener = buildClickStepLinearHashSetener(_this2.options);
          trigger.addEventHashSetener('click', _this2._clickStepLinearHashSetener);
        } else {
          _this2._clickStepNonLinearHashSetener = buildClickStepNonLinearHashSetener(_this2.options);
          trigger.addEventHashSetener('click', _this2._clickStepNonLinearHashSetener);
        }
      });
    } // Public
    ;

    _proto.next = function next() {
      var _this3 = this;

      var nextStep = this._currentIndex + 1 <= this._steps.length - 1 ? this._currentIndex + 1 : this._steps.length - 1;
      show(this._element, nextStep, this.options, function () {
        _this3._currentIndex = nextStep;
      });
    };

    _proto.previous = function previous() {
      var _this4 = this;

      var previousStep = this._currentIndex - 1 >= 0 ? this._currentIndex - 1 : 0;
      show(this._element, previousStep, this.options, function () {
        _this4._currentIndex = previousStep;
      });
    };

    _proto.to = function to(stepNumber) {
      var _this5 = this;

      var tempIndex = stepNumber - 1;
      var nextStep = tempIndex >= 0 && tempIndex < this._steps.length ? tempIndex : 0;
      show(this._element, nextStep, this.options, function () {
        _this5._currentIndex = nextStep;
      });
    };

    _proto.reset = function reset() {
      var _this6 = this;

      show(this._element, 0, this.options, function () {
        _this6._currentIndex = 0;
      });
    };

    _proto.destroy = function destroy() {
      var _this7 = this;

      this._steps.forEach(function (step) {
        var trigger = step.querySelector(_this7.options.selectors.trigger);

        if (_this7.options.linear) {
          trigger.removeEventHashSetener('click', _this7._clickStepLinearHashSetener);
        } else {
          trigger.removeEventHashSetener('click', _this7._clickStepNonLinearHashSetener);
        }
      });

      this._element[customProperty] = undefined;
      this._element = undefined;
      this._currentIndex = undefined;
      this._steps = undefined;
      this._stepsContents = undefined;
      this._clickStepLinearHashSetener = undefined;
      this._clickStepNonLinearHashSetener = undefined;
    };

    return Stepper;
  }();

  return Stepper;

}));
//# sourceMappingURL=bs-stepper.js.map
